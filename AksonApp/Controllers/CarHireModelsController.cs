using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using AksonApp.Models;

namespace AksonApp.Controllers
{
    public class CarHireModelsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: CarHireModels
        public ActionResult Index()
        {
            return View(db.CarHireModel.ToList());
        }

        // GET: CarHireModels/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CarHireModel carHireModel = db.CarHireModel.Find(id);
            if (carHireModel == null)
            {
                return HttpNotFound();
            }
            return View(carHireModel);
        }

        // GET: CarHireModels/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CarHireModels/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Surname,Contact,Email,FromDate,ToDate,RentalPrice,Duration,Referrence,CarId,DateTimeStamp,IsCancelled")] CarHireModel carHireModel)
        {
            if (ModelState.IsValid)
            {
                carHireModel.Id = Guid.NewGuid();
                db.CarHireModel.Add(carHireModel);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(carHireModel);
        }

        // GET: CarHireModels/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CarHireModel carHireModel = db.CarHireModel.Find(id);
            if (carHireModel == null)
            {
                return HttpNotFound();
            }
            return View(carHireModel);
        }

        // POST: CarHireModels/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Surname,Contact,Email,FromDate,ToDate,RentalPrice,Duration,Referrence,CarId,DateTimeStamp,IsCancelled")] CarHireModel carHireModel)
        {
            if (ModelState.IsValid)
            {
                db.Entry(carHireModel).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(carHireModel);
        }

        // GET: CarHireModels/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CarHireModel carHireModel = db.CarHireModel.Find(id);
            if (carHireModel == null)
            {
                return HttpNotFound();
            }
            return View(carHireModel);
        }

        // POST: CarHireModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            CarHireModel carHireModel = db.CarHireModel.Find(id);
            db.CarHireModel.Remove(carHireModel);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult AcceptQuote(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CarHireModel carHireModel = db.CarHireModel.Find(id);
            if (carHireModel == null)
            {
                return HttpNotFound();
            }
            return View(carHireModel);
        }

        [HttpPost, ActionName("AcceptQuote")]
        [ValidateAntiForgeryToken]
        public ActionResult AcceptQuoteConfirmed(Guid id)
        {
            CarHireModel carHireModel = db.CarHireModel.Find(id);
            carHireModel.Accepted = true;
            db.SaveChanges();
            return Json(new { success = true });
        }
        public ActionResult PayFast(string amountTopay, string orderidTopay)
        {
            // Create the order in your DB and get the ID
            string amount = amountTopay;
            string orderId = orderidTopay;
            string name = "Impunga Holdings #" + orderId;
            string description = "Impunga Holdings";

            string site = "";
            string merchant_id = "";
            string merchant_key = "";

            // Check if we are using the test or live system
            string paymentMode = System.Configuration.ConfigurationManager.AppSettings["PaymentMode"];

            if (paymentMode == "test")
            {
                site = "https://sandbox.payfast.co.za/eng/process?";
                merchant_id = "10000100";
                merchant_key = "46f0cd694581a";
                
            }
            else if (paymentMode == "live")
            {
                site = "https://www.payfast.co.za/eng/process?";
                merchant_id = System.Configuration.ConfigurationManager.AppSettings["PF_MerchantID"];
                merchant_key = System.Configuration.ConfigurationManager.AppSettings["PF_MerchantKey"];
            }
            else
            {
                throw new InvalidOperationException("Cannot process payment if PaymentMode (in web.config) value is unknown.");
            }
            // Build the query string for payment site

            StringBuilder str = new StringBuilder();
            str.Append("merchant_id=" + HttpUtility.UrlEncode(merchant_id));
            str.Append("&merchant_key=" + HttpUtility.UrlEncode(merchant_key));
            str.Append("&return_url=" + HttpUtility.UrlEncode(System.Configuration.ConfigurationManager.AppSettings["PF_ReturnURL"]));
            str.Append("&cancel_url=" + HttpUtility.UrlEncode(System.Configuration.ConfigurationManager.AppSettings["PF_CancelURL"]));
            //str.Append("&notify_url=" + HttpUtility.UrlEncode(System.Configuration.ConfigurationManager.AppSettings["PF_NotifyURL"]));

            str.Append("&m_payment_id=" + HttpUtility.UrlEncode(orderId));
            str.Append("&amount=" + HttpUtility.UrlEncode(amount));
            str.Append("&item_name=" + HttpUtility.UrlEncode(name));
            str.Append("&item_description=" + HttpUtility.UrlEncode(description));

            // Redirect to PayFast
            Response.Redirect(site + str.ToString());

            return View();
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
