using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AksonApp.Models;
using PagedList;
using System.Web.SessionState;
using Microsoft.AspNet.Identity;

namespace AksonApp.Controllers
{
    [Authorize]
    public class ManageProductsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        [Authorize]
        public ActionResult MyCart()
        {
            string id = User.Identity.GetUserId();
            return View(db.MyCart.Where(m => m.UserId == id && !m.IsCancelled && m.Status == "Pending").ToList());
        }
        [AllowAnonymous]
        public ActionResult ProductResults()
        {
            return View(db.Products.ToList());
        }
        [HttpPost]
        public ActionResult ProductResults(Guid? itemid)
        {
            if(itemid != null)
            {
                var product = db.Products.Find(itemid);
                if(product != null)
                {
                    bool itemExits = db.MyCart.Any(m => m.productId == itemid);
                    if (!itemExits)
                    {
                        MyCart cart = new MyCart()
                        {
                            Id = Guid.NewGuid(),
                            Quantity = 1,
                            Price = product.ProductPrice,
                            DateTimeStamp = DateTime.Now,
                            IsCancelled = false,
                            Status = "Pending",
                            productId = Guid.Parse(itemid.ToString()),
                            Product = product.ProductName,
                            UserId = User.Identity.GetUserId().ToString(),
                            Image = product.ProductImage
                        };
                        db.MyCart.Add(cart);
                    }
                    else
                    {
                        Guid id = db.MyCart.Where(m => m.productId == itemid).Select(m => m.Id).FirstOrDefault();
                        var updateCart = db.MyCart.Find(id);
                        updateCart.Price += product.ProductPrice;
                        updateCart.Quantity += 1;
                        db.Entry(updateCart).State = EntityState.Modified;
                    }
                   
                    db.SaveChanges();
                    return RedirectToAction("MyCart");
                }
            }
            return View(db.Products.ToList());
        }
        // GET: Products
        public ActionResult Index(int? Sort, int? Order, string search,int? page)
        {
            var list = new SelectList(new[]
            {
                new { ID = "1", Name = "Price" },
                new { ID = "2", Name = "Product" },
                new { ID = "3", Name = "Rating" },
            },
            "ID", "Name", 1);

            ViewBag.List = list;
            var listTwo = new SelectList(new[]
            {
                new { ID = "1", Name = "Asc" },
                new { ID = "2", Name = "Desc" },
            },
         "ID", "Name", 1);

            ViewBag.listTwo = listTwo;
            if(Sort != null && Order != null)
            {
                if(Order == (int)OrderEnum.Asc)
                {
                    if((int)SortEnum.Price == Sort)
                    {
                        return View(db.Products.OrderBy(m => m.ProductPrice).ToList().ToPagedList(page?? 1, 20));
                    }
                    if ((int)SortEnum.Product == Sort)
                    {
                        return View(db.Products.OrderBy(m => m.ProductPrice).ToList().ToPagedList(page ?? 1, 20));
                    }
                    if ((int)SortEnum.Rating == Sort)
                    {
                        return View(db.Products.OrderBy(m => m.ProductPrice).ToList().ToPagedList(page ?? 1, 20));
                    }
                }
                if (Order == (int)OrderEnum.Desc)
                {
                    if ((int)SortEnum.Price == Sort)
                    {
                        return View(db.Products.OrderByDescending(m => m.ProductPrice).ToList().ToPagedList(page ?? 1, 20));
                    }
                    if ((int)SortEnum.Product == Sort)
                    {
                        return View(db.Products.OrderByDescending(m => m.ProductPrice).ToList().ToPagedList(page ?? 1, 20));
                    }
                    if ((int)SortEnum.Rating == Sort)
                    {
                        return View(db.Products.OrderByDescending(m => m.ProductPrice).ToList().ToPagedList(page ?? 1, 20));
                    }
                }
            }
            if (!string.IsNullOrEmpty(search))
            {
                return View(db.Products.Where(m => m.ProductName.Contains(search)).ToList().ToPagedList(page ?? 1, 20));
            }
            return View(db.Products.ToList().ToPagedList(page ?? 1, 20));
        }

        // GET: Products/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Products products = db.Products.Find(id);
            if (products == null)
            {
                return HttpNotFound();
            }
            return View(products);
        }

        // GET: Products/Create
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Products products, HttpPostedFileBase file)
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

                        string path = Path.Combine(Server.MapPath("~/Products"), Path.GetFileName(file.FileName));
                        file.SaveAs(path);

                        products.ProductImage = path.Substring(path.LastIndexOf("\\") + 1);
                    }
                    catch (Exception e)
                    {
                        ViewBag.Error = e.Message;
                        return View(products);
                    }
                }

                products.Id = Guid.NewGuid();
                products.StockRemaining = products.StockAvaliable;
                products.DateTimeStamp = DateTime.Now;
                db.Products.Add(products);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(products);
        }

        // GET: Products/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Products products = db.Products.Find(id);
            if (products == null)
            {
                return HttpNotFound();
            }
            return View(products);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Products products)
        {
            if (ModelState.IsValid)
            {
                db.Entry(products).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("ProductResults");
            }
            return View(products);
        }

        // GET: Products/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Products products = db.Products.Find(id);
            if (products == null)
            {
                return HttpNotFound();
            }
            return View(products);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Products products = db.Products.Find(id);
            db.Products.Remove(products);
            db.SaveChanges();
            return RedirectToAction("ProductResults");
        }

        enum SortEnum
        {
             Price = 1,
             Product = 2,
             Rating = 3,
        };
        enum OrderEnum
        {
             Asc = 1,
             Desc = 2,
        };
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
