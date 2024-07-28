using System.ComponentModel.DataAnnotations;
using System;

namespace ExpressServiceWeb
{
    public class OrderViewModel
    {
        [Required]
        [Display(Name = "Nội dung đơn hàng")]
        public string OrderContent { get; set; }

        [Required]
        [Display(Name = "Thời gian giao hàng")]
        public DateTime DeliveryTime { get; set; }

        [Required]
        [Display(Name = "Địa chỉ giao hàng")]
        public string DeliveryAddress { get; set; }

        [Required]
        [Display(Name = "Thời gian nhận hàng")]
        public DateTime PickupTime { get; set; }

        [Required]
        [Display(Name = "Địa chỉ nhận hàng")]
        public string PickupAddress { get; set; }

        [Required]
        [Display(Name = "Loại phương tiện")]
        public string VehicleTypeId { get; set; }

        [Display(Name = "Mã giảm giá")]
        public string DiscountCode { get; set; }

        [Required]
        [Display(Name = "Khoảng cách")]
        public decimal Distance { get; set; }

        [Required]
        [Display(Name = "Thành tiền")]
        public decimal TotalAmount { get; set; }

        [Display(Name = "Giá sau giảm giá")]
        public decimal DiscountedAmount { get; set; }
        public double DeliveryLatitude { get; set; }
        public double DeliveryLongitude { get; set; }
        public double PickupLatitude { get; set; }
        public double PickupLongitude { get; set; }
        public string driver_id { get; set; }
    }
}

