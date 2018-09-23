using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Configuration;
using System.Net.Mail;
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
                if (file.ContentLength > 0 && file != null)
                {
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
                testDrive.Reference = RandomString();
                db.TestDrive.Add(testDrive);
                db.SaveChanges();

                try
                {
                    SmtpClient smtp = new SmtpClient();
                    var send = (SmtpSection)System.Configuration.ConfigurationManager.GetSection("system.net/mailSettings/smtp");

                    MailMessage mail = new MailMessage();
                    mail.To.Add(testDrive.Email);
                    mail.Subject = "Akson Test Drive Booking Confirmation";
                    mail.From = new MailAddress(send.From);
                    mail.Body = $"Congatulations <br/><br/>" +
                        $"Your test drive booking was successfully and is " +
                        $"scheduled for the {testDrive.Date.ToShortDateString()} and your booking reference is {testDrive.Reference}. <br/><br/>" +
                        "Regards <br/>" +
                        "Akson Sales Team.";
                    mail.IsBodyHtml = true;

                    smtp.Send(mail);

                    Sms sms = new Sms();
                    sms.Send_SMS(testDrive.ContactNumber, $"Akson Test Drive Booking Reference No: {testDrive.Reference}");
                }
                catch(Exception ex)
                {

                }

                return RedirectToAction("Success");
            }

            return View(testDrive);
        }

        public ActionResult Success()
        {
            return View();
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

        private static Random random = new Random();
        public static string RandomString()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, 7)
              .Select(s => s[random.Next(s.Length)]).ToArray());
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
