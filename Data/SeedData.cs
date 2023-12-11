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
                FirstName = "Đoàn",
                LastName = "Quang Huy",
                IsActive = true,
                Avatar = "/upload/avatar/blank_avatar.png"
            };
            var adminUser = new ApplicationUser
            {
                Email = "adminUser@gmail.com",
                UserName = "adminUser",
                FirstName = "Người Dùng",
                LastName = "1",
                IsActive = true,
                Avatar = "/upload/avatar/blank_avatar.png"
            };
            
            var result1 = await userManager.CreateAsync(superAdminUser, superAdminPassword);
            var result2 = await userManager.CreateAsync(adminUser, superAdminPassword);


            if (result1.Succeeded && result2.Succeeded)
            {
                await userManager.AddToRoleAsync(superAdminUser, "SuperAdmin");
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }
        }
    }

}
