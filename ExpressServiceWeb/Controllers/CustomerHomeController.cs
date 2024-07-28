using ExpressServiceWeb.Models;
using System.Web.Mvc;
using System.Linq;

namespace ExpressServiceWeb.Controllers
{
    [Authorize]
    public class CustomerHomeController : Controller
    {
        private ExpressServiceEntities db = new ExpressServiceEntities();
        // GET: CustomerHome
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
        public ActionResult Order()
        {
            return RedirectToAction("Create", "Order");
        }

        // GET: CustomerHome/Profile
        public ActionResult Profile()
        {
            return RedirectToAction("Profile", "Account");
        }

        // GET: CustomerHome/OrderHistory
        public ActionResult History()
        {
            return RedirectToAction("History", "Order");
        }
    }
}
