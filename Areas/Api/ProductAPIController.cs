using LacDau.Data;
using LacDau.Models;
using LacDau.Models.CategoryVM;
using LacDau.Models.ProductVM;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System.Security.Claims;

namespace LacDau.Areas.Api
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ProductAPIController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMemoryCache _memoryCache;
        public ProductAPIController(ApplicationDbContext context, IMemoryCache memoryCache)
        {
            _context = context;
            _memoryCache = memoryCache;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetData()
        {
            JsonResultVM json = new JsonResultVM();
            var rs = await _context.Product.Where(i => i.IsDeleted == false).OrderByDescending(i => i.Id).ToListAsync();
            json.Message = "Success";
            json.StatusCode = 200;
            json.Object = rs;
            return Ok(json);
        }

        [HttpGet("id")]
        [AllowAnonymous]
        public async Task<IActionResult> GetDataById(int id)
        {
            JsonResultVM json = new JsonResultVM();
            var rs = await _context.Product.FirstOrDefaultAsync(i=>i.Id == id);
            if(ReferenceEquals(rs, null))
            {
                json.Message = "NotFoud";
                json.StatusCode = 404;
                json.Object = rs;
                return Ok(json);
            }
            else
            {
                json.Message = "Success";
                json.StatusCode = 200;
                json.Object = rs;
                return Ok(json);
            }
            
        }

        [HttpPost]
        public async Task<IActionResult> SaveData([FromForm]ProductVM vm)
        {
            JsonResultVM json = new JsonResultVM();
            try
            {
                var user = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
                Product product = new Product();
                if (ModelState.IsValid)
                {
                    product = vm;
                    product.CreatedDate = DateTime.Now;
                    _context.Add(product);
                    await _context.SaveChangesAsync();

                    json.Message = user;
                    json.StatusCode = 200;
                    json.Object = product;
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

        [HttpPut]
        public async Task<IActionResult> EditData(ProductVM vm)
        {
            JsonResultVM json = new JsonResultVM();
            try
            {
                Product product = await _context.Product.FirstOrDefaultAsync(i=>i.Id == vm.Id);
                
                if(product == null)
                {
                    json.Message = "Not found";
                    json.StatusCode = 404;
                    json.Object = null;
                    return Ok(json);
                }
                else
                {
                    vm.CreatedDate = product.CreatedDate;
                    _context.Entry(product).CurrentValues.SetValues(vm);
                    await _context.SaveChangesAsync();

                    json.Message = "Not found";
                    json.StatusCode = 200;
                    json.Object = product;
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
        public async Task<IActionResult> DeleteCategory(int id)
        {
            JsonResultVM json = new JsonResultVM();
            Product Product = await _context.Product.FirstOrDefaultAsync(i => i.Id == id);
            if (Product == null)
            {
                json.StatusCode = 404;
                json.Message = "Not Found";
                json.Object = null;
                return Ok(json);
            }
            else
            {
                _context.Product.Remove(Product);
                await _context.SaveChangesAsync();
                json.StatusCode = 202;
                json.Message = "Success";
                json.Object = null;
                return Ok(json);
            }
        }
    }
}
