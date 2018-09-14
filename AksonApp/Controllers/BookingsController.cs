using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Configuration;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using AksonApp.Models;

namespace AksonApp.Controllers
{
    public class BookingsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        
        public ActionResult Index()
        {
            return View(db.Bookings.ToList());
        }

        // GET: Bookings/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Bookings bookings = db.Bookings.Find(id);
            if (bookings == null)
            {
                return HttpNotFound();
            }
            return View(bookings);
        }

        // GET: Bookings/Create
        public ActionResult Create()
        {
            ViewBag.Code = new SelectList(db.PhoneCodes.Where(m => m.code.Length <= 4), "code", "code");
            ViewBag.ServiceType = new SelectList(db.ServiceTypes, "Name", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Bookings bookings, string CountryCode)
        {
            if (ModelState.IsValid)
            {
                bookings.Reference = RandomString();
                bookings.Id = Guid.NewGuid();
                db.Bookings.Add(bookings);
                db.SaveChanges();

                SmtpClient smtp = new SmtpClient();
                var send = (SmtpSection)System.Configuration.ConfigurationManager.GetSection("system.net/mailSettings/smtp");

                MailMessage mail = new MailMessage();
                mail.To.Add(bookings.Email);
                mail.Subject = "Akson Test Drive Booking Confirmation";
                mail.From = new MailAddress(send.From);
                mail.Body = $"Congatulations <br/><br/>" +
                    $"Your {bookings.ServiceType} booking was successfully and is " +
                    $"scheduled for the {bookings.FirstDateOption.ToShortDateString()} and your booking reference is {bookings.Reference}. <br/><br/>" +
                    "Regards <br/>" +
                    "Akson Service Team.";
                mail.IsBodyHtml = true;

                smtp.Send(mail);

                Sms sms = new Sms();
                sms.Send_SMS(bookings.ContactNumber, $"Akson Booking Reference No: {bookings.Reference}");

                return RedirectToAction("Success", "TestDrives");
            }
            ViewBag.Code = new SelectList(db.PhoneCodes, "code", "code");
            ViewBag.ServiceType = new SelectList(db.ServiceTypes, "Name", "Name");
            return View(bookings);
        }

        // GET: Bookings/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Bookings bookings = db.Bookings.Find(id);
            if (bookings == null)
            {
                return HttpNotFound();
            }
            return View(bookings);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Bookings bookings)
        {
            if (ModelState.IsValid)
            {
                db.Entry(bookings).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(bookings);
        }

        // GET: Bookings/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Bookings bookings = db.Bookings.Find(id);
            if (bookings == null)
            {
                return HttpNotFound();
            }
            return View(bookings);
        }

        private static Random random = new Random();
        public static string RandomString()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, 7)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        // POST: Bookings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Bookings bookings = db.Bookings.Find(id);
            db.Bookings.Remove(bookings);
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
