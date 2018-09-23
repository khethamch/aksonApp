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

namespace AksonApp.Controllers
{
    [Authorize]
    public class CarsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        [AllowAnonymous]
        public ActionResult CarResults(int? Sort, int? Order, string search, int? page)
        {
            var list = new SelectList(new[]
            {
                new { ID = "1", Name = "Price" },
                new { ID = "2", Name = "ModelYear" },
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
            if (Sort != null && Order != null)
            {
                if (Order == (int)OrderEnum.Asc)
                {
                    if ((int)SortEnum.Price == Sort)
                    {
                        return View(db.Cars.Where(m => !m.IsForHire).OrderBy(m => m.CarPrice).ToList().ToPagedList(page ?? 1, 20));
                    }
                    if ((int)SortEnum.ModelYear == Sort)
                    {
                        return View(db.Cars.Where(m => !m.IsForHire).OrderBy(m => m.Year).ToList().ToPagedList(page ?? 1, 20));
                    }
                }
                if (Order == (int)OrderEnum.Desc)
                {
                    if ((int)SortEnum.Price == Sort)
                    {
                        return View(db.Cars.Where(m => !m.IsForHire).OrderByDescending(m => m.CarPrice).ToList().ToPagedList(page ?? 1, 20));
                    }
                    if ((int)SortEnum.ModelYear == Sort)
                    {
                        return View(db.Cars.Where(m => !m.IsForHire).OrderByDescending(m => m.Year).ToList().ToPagedList(page ?? 1, 20));
                    }
                }
            }
            if (!string.IsNullOrEmpty(search))
            {
                return View(db.Cars.Where(m => m.Model.Contains(search) && !m.IsForHire).ToList().ToPagedList(page ?? 1, 20));
            }
            return View(db.Cars.Where(m => !m.IsForHire).ToList().ToPagedList(page ?? 1, 20));
        }

        [AllowAnonymous]
        public ActionResult CarResultsForHire(int? Sort, int? Order, string search, int? page)
        {
            var list = new SelectList(new[]
            {
                new { ID = "1", Name = "Price" },
                new { ID = "2", Name = "ModelYear" },
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
            if (Sort != null && Order != null)
            {
                if (Order == (int)OrderEnum.Asc)
                {
                    if ((int)SortEnum.Price == Sort)
                    {
                        return View(db.Cars.Where(m => m.IsForHire).OrderBy(m => m.CarPrice).ToList().ToPagedList(page ?? 1, 20));
                    }
                    if ((int)SortEnum.ModelYear == Sort)
                    {
                        return View(db.Cars.Where(m => m.IsForHire).OrderBy(m => m.Year).ToList().ToPagedList(page ?? 1, 20));
                    }
                }
                if (Order == (int)OrderEnum.Desc)
                {
                    if ((int)SortEnum.Price == Sort)
                    {
                        return View(db.Cars.Where(m => m.IsForHire).OrderByDescending(m => m.CarPrice).ToList().ToPagedList(page ?? 1, 20));
                    }
                    if ((int)SortEnum.ModelYear == Sort)
                    {
                        return View(db.Cars.Where(m => m.IsForHire).OrderByDescending(m => m.Year).ToList().ToPagedList(page ?? 1, 20));
                    }
                }
            }
            if (!string.IsNullOrEmpty(search))
            {
                return View(db.Cars.Where(m => m.Model.Contains(search) && m.IsForHire).ToList().ToPagedList(page ?? 1, 20));
            }
            return View(db.Cars.Where(m => m.IsForHire).ToList().ToPagedList(page ?? 1, 20));
        }
        public ActionResult IndexCars(int? Sort, int? Order, string search, int? page)
        {
            var list = new SelectList(new[]
            {
                new { ID = "1", Name = "Price" },
                new { ID = "2", Name = "ModelYear" },
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
            if (Sort != null && Order != null)
            {
                if (Order == (int)OrderEnum.Asc)
                {
                    if ((int)SortEnum.Price == Sort)
                    {
                        return View(db.Cars.OrderBy(m => m.CarPrice).ToList().ToPagedList(page ?? 1, 20));
                    }
                    if ((int)SortEnum.ModelYear == Sort)
                    {
                        return View(db.Cars.OrderBy(m => m.Year).ToList().ToPagedList(page ?? 1, 20));
                    }
                }
                if (Order == (int)OrderEnum.Desc)
                {
                    if ((int)SortEnum.Price == Sort)
                    {
                        return View(db.Cars.OrderByDescending(m => m.CarPrice).ToList().ToPagedList(page ?? 1, 20));
                    }
                    if ((int)SortEnum.ModelYear == Sort)
                    {
                        return View(db.Cars.OrderByDescending(m => m.Year).ToList().ToPagedList(page ?? 1, 20));
                    }
                }
            }
            if (!string.IsNullOrEmpty(search))
            {
                return View(db.Cars.Where(m => m.Model.Contains(search)).ToList().ToPagedList(page ?? 1, 20));
            }
            return View(db.Cars.ToList().ToPagedList(page ?? 1, 20));
        }

        enum SortEnum
        {
            Price = 1,
            ModelYear = 2,
        };
        enum OrderEnum
        {
            Asc = 1,
            Desc = 2,
        };
        [AllowAnonymous]
        // GET: Cars/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cars cars = db.Cars.Find(id);
            if (cars == null)
            {
                return HttpNotFound();
            }
            return View(cars);
        }

        [HttpPost]
        public ActionResult Details(Guid id, string pickup, string paymentmethode, string name, string surname, string contact, string email, DateTime fromDate, DateTime toDate, bool bluetooth, bool navigation)
        {
            decimal bluetoothPrice = 0, navigationPrice = 0;
            bool blue = false, nav = false;
            if (bluetooth)
            {
                bluetoothPrice = 157;
                blue = true;
            }
            if (navigation)
            {
                navigationPrice = 268;
                nav = true;
            }
            int duration = int.Parse(toDate.Subtract(fromDate).TotalDays.ToString());
            Cars cars = db.Cars.Find(id);
            var carHire = new CarHireModel()
            {
                Name = name,
                Surname = surname,
                Contact = contact,
                Email = email,
                FromDate = fromDate,
                ToDate = toDate,
                Duration = duration,
                RentalPrice = (cars.CarPrice * duration) + navigationPrice + bluetoothPrice,
                Referrence = RandomString(),
                CarId = id,
                PickUpLocation = pickup,
                PayMethode = paymentmethode,
                Bluetooth = blue,
                Navigator = nav
            };

            db.CarHireModel.Add(carHire);
            db.SaveChanges();

            if(carHire.PickUpLocation == "E.F.T")
            {
                TempData["Display"] = "Display";
            }
            return RedirectToAction("Details","CarHireModels", new { id = carHire.Id});
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
            var list = new SelectList(new[]
            {
                new { ID = "Manual", Name = "Manual" },
                new { ID = "Automatic", Name = "Automatic" },
            },
            "Name", "Name", 1);

            ViewBag.List = list;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Cars cars, HttpPostedFileBase fileOutside, HttpPostedFileBase fileInside)
        {
            var list = new SelectList(new[]
            {
                new { ID = "Manual", Name = "Manual" },
                new { ID = "Automatic", Name = "Automatic" },
            },
           "Name", "Name", 1);
            ViewBag.List = list;

            ViewBag.List = list;
            if (ModelState.IsValid)
            {
                if (fileInside.ContentLength > 0 && fileInside != null && fileOutside.ContentLength > 0 && fileOutside != null)
                {
                    string pathInside = Path.Combine(Server.MapPath("~/Cars"), Path.GetFileName(fileInside.FileName));
                    string pathOutside = Path.Combine(Server.MapPath("~/Cars"), Path.GetFileName(fileOutside.FileName));
                    fileOutside.SaveAs(pathOutside);
                    fileInside.SaveAs(pathInside);

                    cars.InteriorImage = pathInside.Substring(pathInside.LastIndexOf("\\") + 1);
                    cars.OutsideImage = pathOutside.Substring(pathOutside.LastIndexOf("\\") + 1);
                }
                db.Cars.Add(cars);
                db.SaveChanges();
                return RedirectToAction("IndexCars");
            }

            return View(cars);
        }

        // GET: Cars/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cars cars = db.Cars.Find(id);
            if (cars == null)
            {
                return HttpNotFound();
            }
            return View(cars);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Cars cars)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cars).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("CarResults");
            }
            return View(cars);
        }

        // GET: Cars/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cars cars = db.Cars.Find(id);
            if (cars == null)
            {
                return HttpNotFound();
            }
            return View(cars);
        }

        // POST: Cars/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Cars cars = db.Cars.Find(id);
            db.Cars.Remove(cars);
            db.SaveChanges();
            return RedirectToAction("CarResults");
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
