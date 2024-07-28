using System.ComponentModel.DataAnnotations;

namespace ExpressServiceWeb
{
    public class RegisterViewModel
    {
        [Required]
        [Display(Name = "Họ và tên")]
        public string FullName { get; set; }

        [Required]
        [Display(Name = "Số điện thoại")]
        public string PhoneNumber { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Mât khẩu")]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Nhập lại mật khẩu")]
        [Compare("Password", ErrorMessage = "Mật khẩu không khớp, vui lòng nhập lại!")]
        public string ConfirmPassword { get; set; }

        [Required]
        [Display(Name = "Vai trò")]
        public string Role { get; set; }

        [Display(Name = "Loại phương tiện")]
        public string VehicleTypeId { get; set; }

        [Display(Name = "Biển số xe")]
        public string LicensePlate { get; set; }
    }
}

