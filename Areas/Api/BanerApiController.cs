using LacDau.Data;
using LacDau.Models;
using LacDau.Models.BannerVM;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Linq;

namespace LacDau.Areas.Api
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class BanerApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMemoryCache _memoryCache;
        private readonly IConfiguration _configuration;

        public BanerApiController(ApplicationDbContext context, IMemoryCache memoryCache, IConfiguration configuration) {
            _context = context;
            _memoryCache = memoryCache;
            _configuration = configuration;
        }

        [HttpGet("banner")]
        public IActionResult GetBaner() {
            JsonResultVM json = new JsonResultVM();
            var cachBaner = _memoryCache.Get(_configuration["MemoriesCache:Banner"]);

            if(cachBaner == null)
            {
                var banner = _context.Banner.Where(i => i.IsDelete == false).ToList();
                List<Banner> banerMain = banner.Where(i => i.Type == 1).ToList();
                List<Banner> banerBottom = banner.Where(i => i.Type == 3).ToList();
                List<Banner> banerRight = banner.Where(i => i.Type == 2).ToList();
                Dictionary<String, List<Banner>> data = new Dictionary<String, List<Banner>>();
                data.Add("main", banerMain);
                data.Add("bottom", banerBottom);
                data.Add("right", banerRight);
                json.Object = data;
                _memoryCache.Set(_configuration["MemoriesCache:Banner"], data);
            }
            else
            {
                json.Object = cachBaner;
            }

            
            json.StatusCode = 200; 
            json.Message = "";
            return Ok(json);
        }
    }
}
