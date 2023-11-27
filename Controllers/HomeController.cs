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

            var cacheKey = _configuration["MemoriesCache:Banner"];
            if (!_cache.TryGetValue(cacheKey, out List<Banner> bannerList))
            {
                bannerList = _context.Banner
                    .Where(i => !i.IsDelete && i.IsActive)
                    .ToList();

                _cache.Set(cacheKey, bannerList);
            }

            ViewBag.BannerMain = bannerList?.Where(i => i.Type == 1).ToList() ?? new List<Banner>();
            ViewBag.BannerRight = bannerList?.Where(i => i.Type == 2).ToList() ?? new List<Banner>();
            ViewBag.BannerBottom = bannerList?.Where(i => i.Type == 3).ToList() ?? new List<Banner>();


            return View();

        }

        [HttpGet("tim/{q}")]
        public async Task<IActionResult> CategoryProduct(string q)
        {
            q = q.ToLower();

            if (q == "admin")
            {
                return Redirect("Admin/Dashboard/Index");
            }
            if (q == "api")
            {
                return Redirect("/api/index.html");
            }

            Category cate = await _context.Category.FirstOrDefaultAsync(c => c.Slug == q);

            return View();
        }

        public IActionResult Card()
        {
            ViewBag.user = HttpContext.User.Identity.Name;
            return PartialView("_card");
        }
    }
}