using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AksonApp.Models;
using System.IO;
using Microsoft.AspNet.Identity;

namespace AksonApp.Controllers
{
    public class PostsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult ManageBlogs()
        {
            return View(db.Post.ToList());
        }
        public ActionResult Like(string id)
        {
            var userid = User.Identity.GetUserId();
            LikeTable like = new LikeTable
            {
                UserId = userid,
                PostId = id
            };
            db.LikeTable.Add(like);
            db.SaveChanges();

            var model = db.Post.Find(Guid.Parse(id));
            return View("Details", model);
        }
        public ActionResult RemoveComment(int? id)
        {
            if(id != null)
            {
                var delete = db.Comment.Find(id);
                if(delete != null)
                {
                    db.Comment.Remove(delete);
                    db.SaveChanges();

                    var findBlog = db.Post.Find(delete.BlogId);
                    if (findBlog != null)
                    {
                        return RedirectToAction("Details", new { id = findBlog.Ref });
                    }
                }
            }
            return null;
        }
        [Authorize]
        public ActionResult AddComment(string id, string CommentContent)
        {
            if (!string.IsNullOrEmpty(id) && !string.IsNullOrEmpty(CommentContent))
            {
                Comment comment = new Comment
                {
                    Name = User.Identity.Name,
                    Date = DateTime.Now,
                    Content = CommentContent,
                    BlogId = Guid.Parse(id)
                };
                db.Comment.Add(comment);
                db.SaveChanges();
            }

            var findBlog = db.Post.Find(Guid.Parse(id));
            if (findBlog != null)
            {
                return RedirectToAction("Details", new { id = findBlog.Ref, postid = findBlog.Id });
            }

            return null;
        }
 
        // GET: Posts/Details/5
        public ActionResult Details(string id,Guid? postid)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Post post = db.Post.Find(postid);
            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }

        public ActionResult DeleteTag(int? id)
        {
            if (id != null)
            {
                Tag t = db.Tag.Find(id);
                db.Tag.Remove(t);
                db.SaveChanges();
            }

            return RedirectToAction("Create");
        }

        public ActionResult Tag(string tag)
        {
            Tag t = new Tag
            {
                KeyWord = tag,
                Author = User.Identity.Name
            };
            db.Tag.Add(t);
            db.SaveChanges();

            return RedirectToAction("Create", new { state = "Valid" });
        }

        [Authorize]
        public ActionResult Create(string state)
        {
            if (string.IsNullOrEmpty(state))
            {
                RemoveIncompleteTag();
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Post post, HttpPostedFileBase file, string tagone)
        {
            if (ModelState.IsValid)
            {
                if (file.ContentLength > 0 && file != null)
                {
                    string ext = Path.GetExtension(file.FileName);
                    if (ext != ".png" && ext != ".PNG" && ext != ".jpg" && ext != ".JPG" && ext != ".jpeg" && ext != ".JPEG")
                    {
                        ViewBag.Error = $"Error, Accepted picture format is .png, .jpg and .jpeg";
                        return View();
                    }
                    try
                    {

                        string path = Path.Combine(Server.MapPath("~/BlogPics"), Path.GetFileName(file.FileName));
                        file.SaveAs(path);

                        post.Image = path.Substring(path.LastIndexOf("\\") + 1);
                    }
                    catch(Exception e)
                    {
                        ViewBag.Error = e.Message;
                        return View(post);
                    }
                }

                post.Ref = post.Title.Replace(" ", "-");
                post.Date = DateTime.Now;
                post.Author = User.Identity.Name;
                post.Id = Guid.NewGuid();
                db.Post.Add(post);
                db.SaveChanges();

                var userLogged = User.Identity.Name;
                var findAndUpdateTag = db.Tag.Where(m => m.Author == userLogged && m.Post == "" || m.Post == null);
                if (findAndUpdateTag != null)
                {
                    foreach (var item in findAndUpdateTag)
                    {
                        Tag t = db.Tag.Find(item.Id);
                        t.Post = post.Id.ToString();
                        db.Entry(t).State = EntityState.Modified;
                    }
                    db.SaveChanges();
                }
                
                return RedirectToAction("ManageBlogs");
            }

            return View(post);
        }

        public void RemoveIncompleteTag()
        {
            var user = User.Identity.Name;

            var count = db.Tag.Count(m => m.Author == user && m.Post == "" || m.Post == null);
            if (count > 0)
            {
                var findTag = db.Tag.Where(m => m.Author == user && m.Post == "" || m.Post == null);
                foreach (var item in findTag)
                {
                    Tag t = db.Tag.Find(item.Id);
                    db.Tag.Remove(t);
                }
                db.SaveChanges();
            }
        }
        
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
