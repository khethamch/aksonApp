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

namespace AksonApp.Controllers
{
    public class TestDrivesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: TestDrives
        public ActionResult Index()
        {
            return View(db.TestDrive.ToList());
        }

        // GET: TestDrives/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TestDrive testDrive = db.TestDrive.Find(id);
            if (testDrive == null)
            {
                return HttpNotFound();
            }
            return View(testDrive);
        }

        // GET: TestDrives/Create
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(TestDrive testDrive, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                if(file.ContentLength > 0 && file != null)
                {
                    string ext = Path.GetExtension(file.FileName);
                    if (ext != ".pdf")
                    {
                        ViewBag.Error = $"Error, Accepted file format is .pdf";
                        return View();
                    }
                    try
                    {
                        string path = Path.Combine(Server.MapPath("~/LicenceCopies"), Path.GetFileName(file.FileName));
                        file.SaveAs(path);

                        testDrive.Attathment = path;
                    }
                    catch (Exception e)
                    {
                        ViewBag.Error = e.Message;
                        return View(testDrive);
                    }
                }

                db.TestDrive.Add(testDrive);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(testDrive);
        }

        // GET: TestDrives/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TestDrive testDrive = db.TestDrive.Find(id);
            if (testDrive == null)
            {
                return HttpNotFound();
            }
            return View(testDrive);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(TestDrive testDrive)
        {
            if (ModelState.IsValid)
            {
                db.Entry(testDrive).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(testDrive);
        }

        // GET: TestDrives/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TestDrive testDrive = db.TestDrive.Find(id);
            if (testDrive == null)
            {
                return HttpNotFound();
            }
            return View(testDrive);
        }

        // POST: TestDrives/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            TestDrive testDrive = db.TestDrive.Find(id);
            db.TestDrive.Remove(testDrive);
            db.SaveChanges();
            return RedirectToAction("Index");
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
