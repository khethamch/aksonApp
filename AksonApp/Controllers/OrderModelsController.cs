using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Configuration;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Mvc;
using AksonApp.Models;
using Microsoft.AspNet.Identity;

namespace AksonApp.Controllers
{
    [Authorize]
    public class OrderModelsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: OrderModels
        public ActionResult Index(string Reference)
        {
            var id = User.Identity.GetUserId();
            if (!string.IsNullOrEmpty(Reference))
            {
                return View(db.OrderModel.Where(m => m.UserId == id && m.Reference == Reference).ToList());
            }
            return View(db.OrderModel.Where(m => m.UserId == id).ToList());
        }

        private static Random random = new Random();
        public static string RandomString()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, 7)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public ActionResult Create()
        {
            string Ref = RandomString();
            var id = User.Identity.GetUserId();
            var model = db.MyCart.Where(m => m.UserId == id && !m.IsCancelled && m.Status == "Pending" && string.IsNullOrEmpty(m.Reference));
            if(model != null)
            {
                foreach (var item in model)
                {
                    MyCart cart = db.MyCart.Find(item.Id);
                    cart.Reference = Ref;
                    db.Entry(cart).State = EntityState.Modified;
                }
                db.SaveChanges();
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(OrderModel orderModel)
        {
            if (ModelState.IsValid)
            {
                orderModel.UserId = db.MyCart.Where(m => m.Reference == orderModel.Reference).Select(m => m.UserId).FirstOrDefault();
                orderModel.TotalItems = db.MyCart.Where(m => m.Reference == orderModel.Reference).Sum(m => m.Quantity);
                orderModel.TotalAmount = db.MyCart.Where(m => m.Reference == orderModel.Reference).Sum(m => m.Price);
                orderModel.Id = Guid.NewGuid();
                orderModel.Status = "Pending";
                db.OrderModel.Add(orderModel);

                var updateCart = db.MyCart.Where(m => m.Reference == orderModel.Reference);
                foreach(var item in updateCart)
                {
                    MyCart cart = db.MyCart.Find(item.Id);
                    cart.Status = "CheckOut";
                    db.Entry(cart).State = EntityState.Modified;
                }
                db.SaveChanges();
                return RedirectToAction("PayFast", new { OrderNumber  = orderModel.Id });
            }

            return View(orderModel);
        }

        // GET: OrderModels/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrderModel orderModel = db.OrderModel.Find(id);
            if (orderModel == null)
            {
                return HttpNotFound();
            }
            return View(orderModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(OrderModel orderModel)
        {
            if (ModelState.IsValid)
            {
                db.Entry(orderModel).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(orderModel);
        }
        public ActionResult PayFast(Guid? OrderNumber)
        {
            if(OrderNumber == null)
            {
                throw new Exception();
            }

            OrderModel model = db.OrderModel.Find(OrderNumber);
            string amount = model.TotalAmount.ToString("0.00");
            string orderId = model.Reference;
            string name = "Akson #" + orderId;
            string description = "Akson Order";

            string site = "";
            string merchant_id = "";
            string merchant_key = "";

            string paymentMode = System.Configuration.ConfigurationManager.AppSettings["PaymentMode"];

            if (paymentMode == "test")
            {
                site = "https://sandbox.payfast.co.za/eng/process?";
                merchant_id = "10000100";
                merchant_key = "46f0cd694581a";

            }
            else
            {
                throw new InvalidOperationException("Cannot process payment if PaymentMode (in web.config) value is unknown.");
            }
   

            StringBuilder str = new StringBuilder();
            str.Append("merchant_id=" + HttpUtility.UrlEncode(merchant_id));
            str.Append("&merchant_key=" + HttpUtility.UrlEncode(merchant_key));
            str.Append("&return_url=" + $"http://www.myprojectsite.co.za/OrderModels/Success?id={OrderNumber}");
            str.Append("&cancel_url=" + $"http://www.myprojectsite.co.za/OrderModels/Cancel?id={OrderNumber}");

            str.Append("&m_payment_id=" + HttpUtility.UrlEncode(orderId));
            str.Append("&amount=" + HttpUtility.UrlEncode(amount));
            str.Append("&item_name=" + HttpUtility.UrlEncode(name));
            str.Append("&item_description=" + HttpUtility.UrlEncode(description));

            Response.Redirect(site + str.ToString());

            return View();
        }

        public ActionResult Success(Guid? id)
        {
            if(id == null)
            {
                throw new Exception();
            }
            OrderModel model = db.OrderModel.Find(id);
            model.Status = "Paid";
            db.Entry(model).State = EntityState.Modified;
            db.SaveChanges();

            try
            {
                var email = db.UserProfile.Where(m => m.UserId == model.UserId).Select(m => m.Email).FirstOrDefault();
                var phone = db.UserProfile.Where(m => m.UserId == model.UserId).Select(m => m.Mobile).FirstOrDefault();
                var name = db.UserProfile.Where(m => m.UserId == model.UserId).Select(m => m.FirstName + " " + m.LastName).FirstOrDefault();

                SmtpClient smtp = new SmtpClient();
                var send = (SmtpSection)System.Configuration.ConfigurationManager.GetSection("system.net/mailSettings/smtp");

                MailMessage mail = new MailMessage();
                mail.To.Add(email);
                mail.Subject = "Akson Payment Confirmation";
                mail.From = new MailAddress(send.From);
                mail.Body = $"Congatulations {name} <br/><br/>" +
                    $"Your payment for order #{model.Reference} was successfully and your order will be shipped to you within 7 days. <br/><br/>" +
                    "Regards <br/>" +
                    "Akson Sales Team.";
                mail.IsBodyHtml = true;

                smtp.Send(mail);

                Sms sms = new Sms();
                sms.Send_SMS(phone, $"Akson, Payment successfull for Order #{model.Reference}");
            }
            catch(Exception ex)
            {

            }

            return RedirectToAction("Index" , new { Reference =  model.Reference});
        }
        public ActionResult Cancel(Guid? id)
        {
            if (id == null)
            {
                throw new Exception();
            }
            OrderModel model = db.OrderModel.Find(id);
            model.Status = "Cancelled";
            db.Entry(model).State = EntityState.Modified;
            db.SaveChanges();

            try
            {
                var email = db.UserProfile.Where(m => m.UserId == model.UserId).Select(m => m.Email).FirstOrDefault();
                var phone = db.UserProfile.Where(m => m.UserId == model.UserId).Select(m => m.Mobile).FirstOrDefault();
                var name = db.UserProfile.Where(m => m.UserId == model.UserId).Select(m => m.FirstName + " " + m.LastName).FirstOrDefault();

                SmtpClient smtp = new SmtpClient();
                var send = (SmtpSection)System.Configuration.ConfigurationManager.GetSection("system.net/mailSettings/smtp");

                MailMessage mail = new MailMessage();
                mail.To.Add(email);
                mail.Subject = "Akson Payment Cancelled";
                mail.From = new MailAddress(send.From);
                mail.Body = $"Hi {name} <br/><br/>" +
                    $"Your payment for order #{model.Reference} was unsuccessfully. <br/><br/>" +
                    "Regards <br/>" +
                    "Akson Sales Team.";
                mail.IsBodyHtml = true;

                smtp.Send(mail);

                Sms sms = new Sms();
                sms.Send_SMS(phone, $"Akson, Payment successfull for Order #{model.Reference}");
            }
            catch (Exception ex)
            {

            }

            return RedirectToAction("Index", new { Reference = model.Reference });
        }
        public ActionResult CartSummary()
        {
            string id = User.Identity.GetUserId();
            return PartialView(db.MyCart.Where(m => m.UserId == id && !m.IsCancelled).ToList());
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
