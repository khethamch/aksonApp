using System;
using System.Collections.Generic;
using System.Linq;
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

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

    }
}