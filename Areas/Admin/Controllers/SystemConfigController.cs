using LacDau.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using LacDau.Services;
using LacDau.Models;
using LacDau.Models.BannerVM;

namespace LacDau.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme, Roles = "SuperAdmin, Admin")]
    public class SystemConfigController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ICommon _common;
        public SystemConfigController(ApplicationDbContext context, ICommon common) {
            _common = common;
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult AddBanner()
        {
            BannerVM bannerVM = new BannerVM();
            return PartialView("_AddBanner", bannerVM);
        }

        public async Task<IActionResult> GetAllBanner()
        {
            JsonResultVM json = new JsonResultVM();
            var rs = await _context.Banner.Where(i=>i.IsDelete == false).ToListAsync();
            json.StatusCode = 200;
            json.Object = rs;
            json.Message = "";
            return Ok(json);

        }

        public async Task<IActionResult> SavedBanner(BannerVM vm)
        {
            JsonResultVM json = new JsonResultVM();
            try
            {
                Banner banner = new Banner();

                if(vm.ContentFile != null)
                {
                    vm.Content = await _common.UploadFileBannerAsync(vm.ContentFile);
                }
                banner = vm;
                banner.CreatedDate = DateTime.Now;
                banner.IsDelete = false;
                banner.IsActive = true;

                _context.Add(banner);
                _context.SaveChanges();

                json.StatusCode = 200;
                json.Object = banner;
                json.Message = "";

                return Ok(json);
            }
            catch (Exception ex)
            {
                json.Message = ex.Message;
                json.StatusCode = 500;
                json.Object = ex;
                return Ok(json);
            }
        }
        public async Task<IActionResult> DeleteBanner(int id)
        {
            Banner banner = await _context.Banner.FirstOrDefaultAsync(i=>i.Id == id);
            banner.IsDelete = true;
            _context.Update(banner);
            _context.SaveChanges();
            return Ok();
        }
        public async Task<IActionResult> ChangeActiveBanner(int id)
        {
            Banner banner = await _context.Banner.FirstOrDefaultAsync(i=>i.Id == id);
            banner.IsActive = !banner.IsActive;
            _context.Update(banner);
            _context.SaveChanges();
            return Ok();
        }
    }
}
