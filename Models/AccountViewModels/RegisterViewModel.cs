using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace LacDau.Models.AccountViewModels
{
    public class RegisterViewModel
    {
        [Display(Name = "Tài khoản")]
        [Required(ErrorMessage = "Tài khoản không được để trống")]
        public string UserName { get; set; }
        [Display(Name = "Mật Khẩu")]
        [Required(ErrorMessage = "Mật khẩu không được để trống")]
        [DataType(DataType.Password)]
        public string PasswordHash { get; set; }
        [DataType(DataType.Password)]
        [Display(Name = "Xác nhận mật khẩu")]
        [Compare("PasswordHash", ErrorMessage = "Mật khẩu không khớp")]
        public string? ConfirmPassword { get; set; }
        public int Province { get; set; }
        public bool Sex { get; set; }
        public int DayDate { get; set; }
        public int MonthDate { get; set; }
        public int YearMonth { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }

        public static implicit operator ApplicationUser(RegisterViewModel vm)
        {
            return new ApplicationUser
            {
                UserName = vm.UserName,
                IsActive = true,
                Email = vm.UserName,
                Province = vm.Province,
                Sex = vm.Sex,
                DayDate = vm.DayDate,
                MonthDate = vm.MonthDate,
                YearMonth = vm.YearMonth,
                FullName = vm.FullName,
                PhoneNumber = vm.Phone,
                Address = vm.Address
            };
        }
    }
}