using Microsoft.AspNetCore.Identity;

namespace LacDau.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Avatar { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public string Address { get; set; } = string.Empty;
        public int Province {  get; set; }
        public bool Sex { get; set; }
        public int DayDate { get; set; }
        public int MonthDate { get; set; }
        public int YearMonth { get; set; }
    }
}
