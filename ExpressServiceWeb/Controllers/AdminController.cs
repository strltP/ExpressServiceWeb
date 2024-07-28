using ExpressServiceWeb.Models;
using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace YourNamespace.Controllers
{
    //[Authorize(Roles = "admin")]
    public class AdminController : Controller
    {
        private ExpressServiceEntities db = new ExpressServiceEntities();

        public ActionResult AdminHome()
        {
            var user = db.Admins.FirstOrDefault(u => u.email == User.Identity.Name);
            if(user != null)
            {
                ViewBag.AdminName = user.full_name;
            }
            
            return View();
        }


        // GET: Admin
        public ActionResult UserManagement()
        {
            var users = db.Users.ToList();
            return View(users);
        }

        // GET: Admin/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(User model)
        {
            if (ModelState.IsValid)
            {
                model.user_id = Guid.NewGuid().ToString();
                db.Users.Add(model);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(model);
        }

        // GET: Admin/Edit/{id}
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }

            var user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }

            return View(user);
        }

        // POST: Admin/Edit/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(User model)
        {
            if (ModelState.IsValid)
            {
                db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(model);
        }

        // GET: Admin/Delete/{id}
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }

            var user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }

            return View(user);
        }

        // POST: Admin/Delete/{id}
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            var user = db.Users.Find(id);
            db.Users.Remove(user);
            db.SaveChanges();
            return RedirectToAction("Index");
        }


        public ActionResult Statistics()
        {

            // Tính toán doanh thu và số đơn hàng
            var totalRevenue = db.Orders.Where(o => o.status == "accepted").Sum(o => o.total_amount);
            var totalOrders = db.Orders.Count();

            // Tạo một model cho view thống kê
            var model = new StatisticsViewModel
            {
                TotalRevenue = totalRevenue,
                TotalOrders = totalOrders
            };

            return View(model);
        }

        public ActionResult DailyStatistics(DateTime? date)
        {
            if (date == null)
            {
                date = DateTime.Today;
            }

            var dailyRevenue = db.Orders
                .Where(o => o.status == "completed" && DbFunctions.TruncateTime(o.created_at) == date)
                .Sum(o => (decimal?)o.total_amount) ?? 0;

            var dailyOrders = db.Orders
                .Where(o => DbFunctions.TruncateTime(o.created_at) == date)
                .Count();

            var model = new DailyStatisticsViewModel
            {
                Date = date.Value,
                DailyRevenue = dailyRevenue,
                DailyOrders = dailyOrders
            };

            return View(model);
        }

        public ActionResult Profile()
        {
            return RedirectToAction("Profile", "Account");
        }
    }
}
