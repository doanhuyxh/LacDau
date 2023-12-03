using LacDau.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LacDau.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using LacDau.Services;

namespace LacDau.Areas.Admin.Controllers

{
    [Area("Admin")]
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme, Roles = "SuperAdmin, Admin")]
    public class OrderController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ICommon _common;
        public OrderController(ApplicationDbContext context, ICommon common) {
            _context = context;
            _common = common;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
