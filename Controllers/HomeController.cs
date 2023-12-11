﻿using LacDau.Data;
using LacDau.Models;
using LacDau.Models.CategoryVM;
using LacDau.Models.ProductVM;
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

            ViewBag.TopSale = (from p in _context.Product
                              where p.IsDeleted == false && p.IsHome
                              select new ProductVM
                              {
                                  Id = p.Id,
                                  Name = p.Name,
                                  Price = p.Price,
                                  ProductImg = _context.ProductImg.Where(i=>i.IsDelete == false && i.ProductId  == p.Id).ToList(),
                                  Slug = p.Slug,
                              }).ToList();

            return View();

        }

        [HttpGet("tim")]
        public async Task<IActionResult> CategoryProduct()
        {
            string q = HttpContext.Request.Query["q"];
            if(q== null|| q== "")
            {
                return View();
            }
            q = q.ToString().ToLower();

            Category cate = await _context.Category.FirstOrDefaultAsync(c => c.Slug == q);

            return View();
        }

        [HttpGet("{slug}")]
        public async Task<IActionResult> Slug(string slug)
        {
            switch (slug)
            {
                case "dang-nhap":
                    return View("_login");
                case "dang-ky":
                    return View("_register");
                case "quen-mat-khau":
                    return View("_forgotPassword");
                default:
                    break;
            }

            ProductVM product = await _context.Product.FirstOrDefaultAsync(i => i.Slug == slug);
            product.ProductImg =  _context.ProductImg.Where(i=>i.ProductId == product.Id&& i.IsDelete == false).ToList();  
            ViewBag.product = product;

            return View("ProductDetail");
        }

        
        [Route("cart.html")]
        public IActionResult Card()
        {
            ViewBag.user = HttpContext.User.Identity.Name;
            return View("_card");
        }
    }
}