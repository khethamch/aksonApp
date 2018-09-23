using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AksonApp.Models;
using Microsoft.AspNet.Identity;

namespace AksonApp.Controllers
{
    public class MyCartsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: MyCarts
        public ActionResult Index()
        {
            return View(db.MyCart.ToList());
        }

        // GET: MyCarts/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MyCart myCart = db.MyCart.Find(id);
            if (myCart == null)
            {
                return HttpNotFound();
            }
            return View(myCart);
        } 

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Guid? cartId, int? newQuantity)
        {
            var myCart = db.MyCart.Find(cartId);
            if (ModelState.IsValid)
            {
                var productPrice = db.Products.Where(m => m.Id == myCart.productId).Select(m => m.ProductPrice).FirstOrDefault();
                myCart.Quantity = int.Parse(newQuantity.ToString());
                myCart.Price = (decimal)newQuantity * productPrice;
                db.Entry(myCart).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(myCart);
        }
        public ActionResult Delete(Guid? cartId)
        {
            if (cartId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MyCart myCart = db.MyCart.Find(cartId);
            if (myCart == null)
            {
                return HttpNotFound();
            }
            return View(myCart);
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
        public ActionResult DeleteConfirmed(Guid cartId)
        {
            MyCart myCart = db.MyCart.Find(cartId);
            db.MyCart.Remove(myCart);
            db.SaveChanges();
            return Json(new { success = true });
        }

        public ActionResult Clear()
        {  
            return View();
        }

        // POST: Bookings/Delete/5
        [HttpPost, ActionName("Clear")]
        [ValidateAntiForgeryToken]
        public ActionResult ClearConfirmed()
        {
            var id = User.Identity.GetUserId();
            var clearAll = db.MyCart.Where(m => m.UserId == id && !m.IsCancelled);
            foreach(var item in clearAll)
            {
                MyCart myCart = db.MyCart.Find(item.Id);
                db.MyCart.Remove(myCart);
            }
           
            db.SaveChanges();
            return Json(new { success = true });
        }
        public ActionResult Plus(Guid id)
        {
            MyCart myCart = db.MyCart.Find(id);
            myCart.Quantity += 1;
            var price = db.Products.Where(m => m.Id == myCart.productId).Select(m => m.ProductPrice).FirstOrDefault();
            myCart.Price += price;
            db.Entry(myCart).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("MyCart", "ManageProducts");
        }
        public ActionResult Minus(Guid id)
        {
            MyCart myCart = db.MyCart.Find(id);
            if(myCart.Quantity == 1)
            {
                MinusOneLeft(id);
                return RedirectToAction("MyCart", "ManageProducts");
            }
            myCart.Quantity -= 1;
            var price = db.Products.Where(m => m.Id == myCart.productId).Select(m => m.ProductPrice).FirstOrDefault();
            myCart.Price -= price;
            db.Entry(myCart).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("MyCart", "ManageProducts");
        }
        [HttpPost]
        public ActionResult MinusOneLeft(Guid id)
        {
            MyCart myCart = db.MyCart.Find(id);
            db.MyCart.Remove(myCart);
            db.SaveChanges();
            return RedirectToAction("MyCart", "ManageProducts");
        }

        public ActionResult CheckOut()
        {
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
