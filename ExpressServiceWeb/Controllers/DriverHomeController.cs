using ExpressServiceWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ExpressServiceWeb.Controllers
{
    public class DriverHomeController : Controller
    {
        private ExpressServiceEntities db = new ExpressServiceEntities();
        // GET: DriverHome
        // GET: DriverHome
        public ActionResult Index()
        {
            var user = db.Users.FirstOrDefault(u => u.email == User.Identity.Name);
            if (user != null)
            {
                ViewBag.UserName = user.full_name;
                //  ViewBag.Avatar = user.; // Assuming have an avatar field
            }
            return View();
        }

        // GET: DriverHome/AcceptOrder
        public ActionResult AcceptOrder()
        {
            return RedirectToAction("ListOrders", "Order");
        }
        // GET: DriverHome/Profile
        public ActionResult Profile()
        {
            return RedirectToAction("Profile", "Account");
        }
        public ActionResult AcceptedOrders()
        {
            return RedirectToAction("AcceptedOrders", "Order");
        }
    }
}