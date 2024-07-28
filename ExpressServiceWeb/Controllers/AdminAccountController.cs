using ExpressServiceWeb.Models;
using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace ExpressServiceWeb.Controllers
{
    public class AdminAccountController : Controller
    {
        private ExpressServiceEntities db = new ExpressServiceEntities();

        // GET: AdminAccount/Login
        public ActionResult LoginAdmin()
        {
            return View();
        }

        // POST: AdminAccount/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LoginAdmin(AdminLoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var admin = db.Admins.FirstOrDefault(a => a.email == model.Email && a.password == model.Password);
                if (admin != null)
                {
                    FormsAuthentication.SetAuthCookie(admin.email, false);
                    return RedirectToAction("AdminHome", "Admin");
                }
                else
                {
                    ModelState.AddModelError("", "Thông tin đăng nhập không hợp lệ.");
                }
            }
            return View(model);
        }

        // GET: AdminAccount/Logout
        public ActionResult LogoutAdmin()
        {
           // FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }


        public ActionResult Profile()
        {
            var user = db.Users.FirstOrDefault(u => u.email == User.Identity.Name);
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var model = new ProfileViewModel
            {
                FullName = user.full_name,
                PhoneNumber = user.phone_number,
                Email = user.email
            };

            return View(model);
        }

        // POST: Account/Profile
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult EditProfile(ProfileViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = db.Admins.FirstOrDefault(u => u.email == User.Identity.Name);
                if (user != null)
                {
                    user.full_name = model.FullName;
                    user.email = model.Email;
                    db.SaveChanges();
                    ViewBag.Message = "Thông tin cá nhân đã được cập nhật thành công!";
                }
            }
            return View(model);
        }
    }
}
