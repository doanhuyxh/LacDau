using System.ComponentModel.DataAnnotations;

namespace LacDau.Models.AccountViewModels
{
    public class UserProFileViewModel
    {
        public string ApplicationUserId { get; set; }
        [Display(Name = "Ảnh đại diện")]
        public IFormFile AvatarFile { get; set; }
        public string AvatarPath { get; set; }
        [Display(Name = "Nhóm người dùng")]
        public string Role { set; get; }
        [Display(Name = "Tài khoản")]
        public string UserName { set; get; }
        [Display(Name = "Mật khẩu")]
        [DataType(DataType.Password)]
        public string Password { set; get; }
        [Display(Name = "Mail")]
        [Required(ErrorMessage ="Mail không được để trống")]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", ErrorMessage = "Không đúng định dạng mail")]
        public string Email { set; get; }
        [Display(Name = "Số điện thoại")]
        [Required(ErrorMessage = "Số điện thoại không được để trống")]
        [RegularExpression(@"^(?:\+88|01)?\d{11}$", ErrorMessage = "không đúng định dạng số điện thoại")]
        public string PhoneNumber { set; get; }
        [Display(Name = "Họ")]
        [Required(ErrorMessage = "Không được để trống")]
        public string FisrtName { set; get; }
        [Display(Name = "Tên")]
        [Required(ErrorMessage = "Khồn được để trống")]
        public string LastName { set; get; }
        [DataType(DataType.Password)]
        [Display(Name = "Xác nhận mật khẩu")]
        [Compare("Password", ErrorMessage = "Mật khẩu xác nhận không khớp.")]
        public string PasswordCompere { set; get; }
        public bool IsActive { get; set; } = true;

        [Display(Name ="Địa chỉ")]
        public string? Address { get; set; }
    }
}
