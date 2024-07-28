using ExpressServiceWeb.Models;
using ExpressServiceWeb;
using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;


namespace ExpressServiceWeb.Controllers
{
    //[Authorize(Roles = "customer")]
    public class OrderController : Controller
    {
        private ExpressServiceEntities db = new ExpressServiceEntities();

        // GET: Order/Create
        public ActionResult Create()
        {
            var userId = Session["UserId"];
            var userRole = Session["UserRole"];

            if (userId == null || userRole == null || userRole.ToString() != "customer")
            {
                return RedirectToAction("Login", "Account");
            }

            ViewBag.VehicleTypeId = new SelectList(db.VehicleTypes, "vehicle_type_id", "vehicle_name");
            return View(new OrderViewModel
            {
                DeliveryTime = DateTime.Now,
                PickupTime = DateTime.Now.AddMinutes(30)
            });
        }

        [HttpPost]
        public JsonResult ApplyDiscount(string discountCode, decimal totalAmount)
        {
            var discount = db.DiscountCodes.FirstOrDefault(d => d.discount_id == discountCode);
            if (discount != null && discount.expiration_date > DateTime.Now)
            {
                var discountAmount = (totalAmount * discount.discount_percentage) / 100;
                var discountedAmount = totalAmount - discountAmount;
                return Json(new { success = true, discountedAmount });
            }
            return Json(new { success = false, message = "Mã giảm giá không hợp lệ hoặc đã hết hạn." });
        }

        // POST: Order/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(OrderViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = db.Users.FirstOrDefault(u => u.email == User.Identity.Name);
                if (user == null)
                {
                    return RedirectToAction("Login", "Account");
                }

                // Kiểm tra mã giảm giá
                var discount = db.DiscountCodes.FirstOrDefault(d => d.discount_id == model.DiscountCode);
                if (discount != null && discount.expiration_date > DateTime.Now)
                {
                    var discountAmount = (model.TotalAmount * discount.discount_percentage) / 100;
                    model.DiscountedAmount = model.TotalAmount - discountAmount;
                }
                else if (!string.IsNullOrEmpty(model.DiscountCode))
                {
                    ModelState.AddModelError("DiscountCode", "Mã giảm giá không hợp lệ hoặc đã hết hạn.");
                    ViewBag.VehicleTypeId = new SelectList(db.VehicleTypes, "vehicle_type_id", "vehicle_name", model.VehicleTypeId);
                    return View(model);
                }

                // Ensure the DateTime values are within the valid range for SQL Server datetime
                DateTime minDateTime = new DateTime(1753, 1, 1);
                DateTime maxDateTime = new DateTime(9999, 12, 31);

                DateTime deliveryTime = model.DeliveryTime < minDateTime ? minDateTime : (model.DeliveryTime > maxDateTime ? maxDateTime : model.DeliveryTime);
                DateTime pickupTime = model.PickupTime < minDateTime ? minDateTime : (model.PickupTime > maxDateTime ? maxDateTime : model.PickupTime);

                var order = new Order
                {
                    order_id = Guid.NewGuid().ToString(),
                    customer_id = user.user_id,
                    order_content = model.OrderContent,
                    delivery_time = deliveryTime,
                    delivery_address = model.DeliveryAddress,
                    pickup_time = pickupTime,
                    pickup_address = model.PickupAddress,
                    vehicle_type_id = model.VehicleTypeId,
                    discount_code = model.DiscountCode,
                    distance = model.Distance,
                    total_amount = model.TotalAmount,
                    DiscountedAmount = model.DiscountedAmount,
                    status = "pending",
                    created_at = DateTime.Now,
                    DeliveryLatitude = model.DeliveryLatitude,
                    DeliveryLongitude = model.DeliveryLongitude,
                    PickupLatitude = model.PickupLatitude,
                    PickupLongitude = model.PickupLongitude
                };

                db.Orders.Add(order);
                db.SaveChanges();

                return RedirectToAction("Index", "CustomerHome");
            }

            ViewBag.VehicleTypeId = new SelectList(db.VehicleTypes, "vehicle_type_id", "vehicle_name", model.VehicleTypeId);
            return View(model);
        }
        public ActionResult History()
        {
            var user = db.Users.FirstOrDefault(u => u.email == User.Identity.Name);
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var orders = db.Orders.Where(o => o.customer_id == user.user_id).ToList();
            return View(orders);
        }



        public ActionResult ListOrders()
        {
            var availableOrders = db.Orders.Where(o => o.status == "pending").ToList();
            return View(availableOrders);
        }

        // POST: Order/AcceptOrder
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AcceptOrder(string orderId)
        {
            var order = db.Orders.FirstOrDefault(o => o.order_id == orderId);

            if (order != null && order.status == "pending")
            {
                order.driver_id = db.Users.FirstOrDefault(u => u.email == User.Identity.Name)?.user_id;
                order.status = "accepted";
                db.SaveChanges();
                return RedirectToAction("ListOrders");
            }

            return RedirectToAction("ListOrders", new { error = "Unable to accept order." });
        }

        // GET: Order/AcceptedOrders
        public ActionResult AcceptedOrders()
        {
            var driverId = db.Users.FirstOrDefault(u => u.email == User.Identity.Name)?.user_id;
            var acceptedOrders = db.Orders.Where(o => o.driver_id == driverId && o.status == "accepted").ToList();
            return View(acceptedOrders);
        }
    }

}
