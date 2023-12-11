using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using LacDau.Data;
using LacDau.Models;
using LacDau.Models.AccountViewModels;
using LacDau.Services;

namespace LacDau.Controllers
{
    public static class CustomClaimTypes
    {
        public const string FullName = "FullName";
    }


    [AllowAnonymous]
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<LoginViewModel> _logger;
        private readonly IConfiguration _iConfiguration;
        private readonly UserManager<ApplicationUser> _userManager;

        public AccountController(UserManager<ApplicationUser> userManager, ApplicationDbContext context, SignInManager<ApplicationUser> signInManager, ILogger<LoginViewModel> logger, IConfiguration iConfiguration)
        {
            _context = context;
            _signInManager = signInManager;
            _logger = logger;
            _iConfiguration = iConfiguration;
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult Login() { return View(); }

        [HttpGet]
        public IActionResult Register() { return View(); }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await _userManager.FindByNameAsync(model.UserName);
            string fullname = "";
            try
            {
                fullname += user.FirstName + " " + user.LastName;
            }
            catch (Exception ex)
            {
                fullname = user.Email;
            }

            if (user != null && !user.IsActive)
            {
                ModelState.AddModelError(string.Empty, "Tài khoản bị khoá");
                return View(model);
            }

            var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                var role = await _userManager.GetRolesAsync(user);
                try
                {

                    var claims = new List<Claim> {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, role.FirstOrDefault()!),
                };

                    // Xây dựng ClaimsIdentity
                    var claimsIdentity = new ClaimsIdentity(
                        claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    // Thiết lập các thuộc tính xác thực (nếu có)
                    var authProperties = new AuthenticationProperties
                    {
                        IsPersistent = true, // Thiết lập cho phép lưu cookie vĩnh viễn (remember me)
                        ExpiresUtc = DateTimeOffset.UtcNow.AddHours(1), // Thiết lập thời gian hết hạn sau 2 giờ
                    };


                    // Đăng ký phiên đăng nhập hiện tại vào HttpContext
                    await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity),
                        authProperties);

                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("err", ex.Message);
                    return View(model);
                }


                if (role.Contains("SuperAdmin") || role.Contains("Admin"))
                {
                    return Redirect("/Admin/Dashboard/Index");
                }
                else
                {
                    return Redirect("/Home/Index");
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Tài khoản hoặc mật khẩu bị lỗi");
                return View(model);
            }

        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = model;
                var result = await _userManager.CreateAsync(user, model.PasswordHash);

                if (result.Succeeded)
                {
                    // Set role member
                    await _userManager.AddToRoleAsync(user, "Member");
                    // Automatically sign in the user
                    await _signInManager.SignInAsync(user, isPersistent: false);

                    return RedirectToAction("Index", "Home");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home"); // Chuyển hướng đến trang chủ hoặc trang khác
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
