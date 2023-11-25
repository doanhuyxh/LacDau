using LacDau.Data;
using LacDau.Models;
using LacDau.Models.CategoryVM;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace LacDau.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> CategoryProduct(string slug)
        {
            slug = slug.ToLower();

            if(slug == "admin")
            {
                return Redirect("Admin/Dashboard/Index");
            }
            if(slug == "api")
            {
                return Redirect("/api/index.html");
            }

            Category cate = await _context.Category.FirstOrDefaultAsync(c => c.Slug == slug);
            

            return View();
        }

        public IActionResult Card()
        {
            ViewBag.user = HttpContext.User.Identity.Name;
            return PartialView("_card");
        }
    }
}