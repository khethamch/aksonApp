using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Configuration;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using AksonApp.Models;

namespace AksonApp.Controllers
{
    public class HomeController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Calculator()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Calculator(CalcDetails calc)
        {
            if(calc.prime == 1)
            {
                calc.prime = Convert.ToDecimal(10.25);
            }
            if (calc.prime == 2)
            {
                calc.prime = Convert.ToDecimal(11.25);
            }
            if (calc.prime == 3)
            {
                calc.prime = Convert.ToDecimal(12.25);
            }
            if (calc.prime == 4)
            {
                calc.prime = Convert.ToDecimal(13.25);
            }
            if (calc.prime == 5)
            {
                calc.prime = Convert.ToDecimal(14.25);
            }
            calc.ballomPrice = (calc.ballon / 100) * calc.purchasePrice;
            calc.Installment = (((calc.purchasePrice - calc.ballomPrice - calc.deposit) / calc.months) * (calc.prime / 100));

            return View(calc);
        }
        public ActionResult Index()
        {
            return View(db.Post.OrderByDescending(m => m.Date).Take(3));
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
        [HttpGet]
        public ActionResult ContactDealer()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ContactDealer(ContactModel contact)
        {
            if (ModelState.IsValid)
            {
                SmtpClient smtp = new SmtpClient();
                var send = (SmtpSection)System.Configuration.ConfigurationManager.GetSection("system.net/mailSettings/smtp");

                MailMessage mail = new MailMessage();
                mail.To.Add(contact.Email);
                mail.Subject = "Online Enquiry Response";
                mail.From = new MailAddress(send.From);
                mail.Body = $"Dear {contact.Name} <br/><br/>" +
                    $"We receive your enquiry and one of our friendly staff will contact you shortly.  <br/><br/>" +
                    "Regards <br/>" +
                    "Akson Service Team.";
                mail.IsBodyHtml = true;

                //smtp.Send(mail);

                TempData["Success"] = "Your enquiry was sent successfully and one of our friendly staff will contact you shortly.";
                return Json(new { success = true });
            }
            return View(contact);

        }

    }
}