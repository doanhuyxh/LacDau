using LacDau.Data;
using LacDau.Models;
using LacDau.Models.TrademarkVM;
using LacDau.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LacDau.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "SuperAdmin, Admin")]
    public class TrademarkController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ICommon _icommon;
        public TrademarkController(ApplicationDbContext context, ICommon common)
        {
            _context = context;
            _icommon = common;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetData()
        {
            JsonResultVM json = new JsonResultVM();
            var rs = await _context.Trademark.Where(i => i.IsDeleted == false).OrderByDescending(i=>i.Id).ToListAsync();
            json.Message = "Success";
            json.StatusCode = 200;
            json.Object = rs;
            return Ok(json);
        }

        [HttpGet]
        public IActionResult AddData()
        {
            TrademarkVM trademark = new TrademarkVM();
            return View(trademark);
        }

        [HttpPost]
        public async Task<IActionResult> AddData(TrademarkVM vm)
        {
            JsonResultVM json = new JsonResultVM();
            try
            {
                if (ModelState.IsValid)
                {
                    Trademark trademark = new Trademark();
                    if (vm.Id == 0)
                    {
                        trademark = vm;
                        trademark.CreatedDate = DateTime.Now;
                        if (vm.LogoImg != null)
                        {
                            trademark.Logo = await _icommon.UploadTrademarkAsync(vm.LogoImg);
                        }
                        _context.Add(trademark);
                        await _context.SaveChangesAsync();

                        json.Message = "Success";
                        json.StatusCode = 200;
                        json.Object = trademark;
                        return Ok(json);
                    }
                    else
                    {
                        trademark = await _context.Trademark.FirstOrDefaultAsync(i => i.Id == vm.Id);
                        if (vm.LogoImg != null)
                        {
                            vm.Logo = await _icommon.UploadTrademarkAsync(vm.LogoImg);
                        }
                        vm.CreatedDate = trademark.CreatedDate;
                        _context.Entry(trademark).CurrentValues.SetValues(vm);
                        await _context.SaveChangesAsync();

                        json.Message = "Success";
                        json.StatusCode = 200;
                        json.Object = trademark;
                        return Ok(json);
                    }
                }
                else
                {
                    json.Message = ModelState.ValidationState.ToString();
                    json.StatusCode = 500;
                    json.Object = null;
                    return Ok(json);
                }

            }
            catch (Exception ex)
            {
                json.Message = ex.Message;
                json.StatusCode = 500;
                json.Object = ex;
                return Ok(json);
            }
        }
        [HttpGet]
        public async Task<IActionResult> EditData(int id)
        {
            TrademarkVM trademark = await _context.Trademark.FirstOrDefaultAsync(i=>i.Id == id);
            return View(trademark);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteTrademark(int id)
        {
            JsonResultVM json = new JsonResultVM();
            Trademark trademark = await _context.Trademark.FirstOrDefaultAsync(i => i.Id == id);
            if (trademark == null)
            {
                json.StatusCode = 404;
                json.Message = "Not Found";
                json.Object = null;
                return Ok(json);
            }
            else
            {
                _context.Trademark.Remove(trademark);
                await _context.SaveChangesAsync();
                json.StatusCode = 202;
                json.Message = "Success";
                json.Object = null;
                return Ok(json);
            }
        }
    }
}
