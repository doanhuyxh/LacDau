using Microsoft.AspNetCore.Identity;

namespace LacDau.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Avatar { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public string Address { get; set; } = string.Empty;
    }
}
