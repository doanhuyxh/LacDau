using Microsoft.AspNetCore.Identity;
using LacDau.Models;

namespace LacDau.Data
{
    public interface IIdentityDataInitializer
    {
        Task SeedData(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager);
    }

    public class IdentityDataInitializer : IIdentityDataInitializer
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;

        public IdentityDataInitializer(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }

        public async Task SeedData(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {

            // Add roles          
            await roleManager.CreateAsync(new IdentityRole("SuperAdmin"));
            await roleManager.CreateAsync(new IdentityRole("Admin"));
            await roleManager.CreateAsync(new IdentityRole("Member"));

            // Add super admin user
            var superAdminEmail = _configuration["SuperAdminDefaultOption:Email"];
            var superAdminUserName = _configuration["SuperAdminDefaultOption:Username"];
            var superAdminPassword = _configuration["SuperAdminDefaultOption:Password"];
            var superAdminUser = new ApplicationUser
            {
                Email = superAdminEmail,
                UserName = superAdminUserName,
                FullName = "Đoàn Quang Huy",
                IsActive = true,
                Avatar = "/upload/avatar/blank_avatar.png",
                DayDate = 18,
                MonthDate = 02,
                YearMonth = 2003,
                Address = "adda",
                Province = 28,
                Sex = true,
            };
            var result1 = await userManager.CreateAsync(superAdminUser, superAdminPassword);


            if (result1.Succeeded)
            {
                await userManager.AddToRoleAsync(superAdminUser, "SuperAdmin");
            }
        }
    }

}
