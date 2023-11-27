using LacDau.Data;
using LacDau.Models;
using LacDau.Models.CategoryVM;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System.Diagnostics;

namespace LacDau.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly IMemoryCache _cache;
        private readonly IConfiguration _configuration;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context, IMemoryCache cache, IConfiguration configuration1)
        {
            _logger = logger;
            _context = context;
            _cache = cache;
            _configuration = configuration1;
        }

        public IActionResult Index()
        {

            var cacheData = _cache.Get(_configuration["MemoriesCache:Banner"]);
            if (cacheData == null)
            {
                List<Banner> bannerList = _context.Banner.Where(i => i.IsDelete == false && i.IsActive == true).ToList();
                ViewBag.BannerMain = bannerList.Where(i => i.Type == 1).ToList();
                ViewBag.BannerRight = bannerList.Where(i => i.Type == 2).ToList();
                ViewBag.BannerBottom = bannerList.Where(i => i.Type == 3).ToList();
                _cache.Set(_configuration["MemoriesCache:Banner"], bannerList);
            }
            else
            {
                List<Banner> bannerList = (List<Banner>)cacheData;
                ViewBag.BannerMain = bannerList.Where(i => i.Type == 1).ToList();
                ViewBag.BannerRight = bannerList.Where(i => i.Type == 2).ToList();
                ViewBag.BannerBottom = bannerList.Where(i => i.Type == 3).ToList();
            }
            return View();

        }
        public async Task<IActionResult> CategoryProduct(string slug)
        {
            slug = slug.ToLower();

            if (slug == "admin")
            {
                return Redirect("Admin/Dashboard/Index");
            }
            if (slug == "api")
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