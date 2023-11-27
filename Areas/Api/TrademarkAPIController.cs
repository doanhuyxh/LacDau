using LacDau.Data;
using LacDau.Models;
using LacDau.Models.TrademarkVM;
using LacDau.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace LacDau.Areas.Api
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class TrademarkAPIController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ICommon _icommon;
        private readonly IConfiguration _configuration;
        private readonly IMemoryCache _memoryCache;

        public TrademarkAPIController(ApplicationDbContext context, ICommon common, IConfiguration configuration, IMemoryCache memoryCache)
        {
            _context = context;
            _icommon = common;
            _configuration = configuration;
            _memoryCache = memoryCache;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetData()
        {
            JsonResultVM json = new JsonResultVM();

            var rs = await _context.Trademark.Where(i => i.IsDeleted == false).ToListAsync();
            json.Message = "Success";
            json.StatusCode = 200;
            json.Object = rs;
            return Ok(json);
        }

        [HttpPost]
        public async Task<IActionResult> AddData([FromForm] TrademarkVM vm)
        {
            JsonResultVM json = new JsonResultVM();
            try
            {
                Trademark trademark = new Trademark();
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
            catch (Exception ex)
            {
                json.Message = ex.Message;
                json.StatusCode = 500;
                json.Object = ex;
                return Ok(json);
            }
        }

        [HttpPut]
        public async Task<IActionResult> EditData([FromForm] TrademarkVM vm)
        {
            JsonResultVM json = new JsonResultVM();
            try
            {
                if (ModelState.IsValid)
                {
                    Trademark trademark = await _context.Trademark.FirstOrDefaultAsync(i => i.Id == vm.Id);
                    if (vm.LogoImg != null)
                    {
                        trademark.Logo = await _icommon.UploadTrademarkAsync(vm.LogoImg);
                    }
                    vm.CreatedDate = trademark.CreatedDate;
                    _context.Entry(trademark).CurrentValues.SetValues(vm);
                    await _context.SaveChangesAsync();

                    json.Message = "Success";
                    json.StatusCode = 200;
                    json.Object = trademark;
                    return Ok(json);
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
