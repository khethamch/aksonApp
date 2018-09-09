using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
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

        // POST: TestDrives/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,FirstName,LastName,ContactNumber,Email,Model,Make,Licence,Attathment,IsCancelled,DateTimeStamp")] TestDrive testDrive)
        {
            if (ModelState.IsValid)
            {
                testDrive.Id = Guid.NewGuid();
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

        // POST: TestDrives/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,FirstName,LastName,ContactNumber,Email,Model,Make,Licence,Attathment,IsCancelled,DateTimeStamp")] TestDrive testDrive)
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
