using ExpressServiceWeb.Models;
using ExpressServiceWeb;
using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;

namespace YourNamespace.Controllers
{
    public class AccountController : Controller
    {
        private ExpressServiceEntities db = new ExpressServiceEntities();

        public ActionResult Register()
        {
            ViewBag.VehicleTypeId = new SelectList(db.VehicleTypes, "vehicle_type_id", "vehicle_name");
            return View();
        }

        // POST: Account/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Check if email already exists
                var existingUser = db.Users.FirstOrDefault(u => u.email == model.Email);
                if (existingUser != null)
                {
                    ModelState.AddModelError("", "Email already exists.");
                    ViewBag.VehicleTypeId = new SelectList(db.VehicleTypes, "vehicle_type_id", "vehicle_name", model.VehicleTypeId);
                    return View(model);
                }

                // Create new user
                var user = new User
                {
                    user_id = Guid.NewGuid().ToString(),
                    full_name = model.FullName,
                    phone_number = model.PhoneNumber,
                    email = model.Email,
                    password = model.Password, // hash the password before storing it
                    role = model.Role,
                    created_at = DateTime.Now
                };

                if (model.Role == "driver")
                {
                    var driver = new Driver
                    {
                        driver_id = user.user_id,
                        vehicle_type_id = model.VehicleTypeId,
                        license_plate = model.LicensePlate
                    };
                    db.Drivers.Add(driver);
                }

                db.Users.Add(user);
                db.SaveChanges();

                // Redirect after successful registration
                return RedirectToAction("Login", "Account");
            }

            ViewBag.VehicleTypeId = new SelectList(db.VehicleTypes, "vehicle_type_id", "vehicle_name", model.VehicleTypeId);
            return View(model);
        }


        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        // POST: Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var user = db.Users.FirstOrDefault(u => u.email == model.Email && u.password == model.Password);

                if (user != null)
                {
                    FormsAuthentication.SetAuthCookie(user.email, false);

                    // Lưu trữ thông tin người dùng vào Session
                    Session["UserId"] = user.user_id;
                    Session["UserName"] = user.full_name;
                    Session["UserRole"] = user.role;

                    if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/") && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        if (user.role == "customer")
                        {
                            return RedirectToAction("Index", "CustomerHome");
                        }
                        else if (user.role == "driver")
                        {
                            return RedirectToAction("Index", "DriverHome");
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Invalid login attempt.");
                }
            }

            return View(model);
        }

        // GET: Account/Logout
        public ActionResult Logout()
        {
            //FormsAuthentication.SignOut();
            Session.Clear();
            return RedirectToAction("Index", "Home");
        }
        [Authorize]
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
        public ActionResult Profile(ProfileViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = db.Users.FirstOrDefault(u => u.email == User.Identity.Name);
                if (user != null)
                {
                    user.full_name = model.FullName;
                    user.phone_number = model.PhoneNumber;
                    db.SaveChanges();
                    ViewBag.Message = "Thông tin cá nhân đã được cập nhật thành công!";
                }
            }
            return View(model);
        }
    }
}
