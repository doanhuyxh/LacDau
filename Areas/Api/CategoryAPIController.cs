﻿using LacDau.Data;
using LacDau.Models;
using LacDau.Models.CategoryVM;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LacDau.Areas.Api
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class CategoryAPIController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public CategoryAPIController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetData()
        {
            JsonResultVM json = new JsonResultVM();
            var rs = await _context.Category.Where(i => i.IsDeleted == false).OrderByDescending(i => i.Id).ToListAsync();
            json.Message = "Success";
            json.StatusCode = 200;
            json.Object = rs;
            return Ok(json);
        }

        [HttpPost]
        public async Task<IActionResult> AddData(CategoryVM vm)
        {
            JsonResultVM json = new JsonResultVM();
            try
            {
                Category Category = new Category();
                if (ModelState.IsValid)
                {
                    Category = vm;
                    Category.CreatedDate = DateTime.Now;
                    _context.Add(Category);
                    await _context.SaveChangesAsync();

                    json.Message = "Success";
                    json.StatusCode = 200;
                    json.Object = Category;
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
        public async Task<IActionResult> EditData(CategoryVM vm)
        {
            JsonResultVM json = new JsonResultVM();
            try
            {
                Category Category = await _context.Category.FirstOrDefaultAsync(i=>i.Id == vm.Id);
                
                if(Category == null)
                {
                    json.Message = "Not found";
                    json.StatusCode = 400;
                    json.Object = null;
                    return Ok(json);
                }
                else
                {
                    vm.CreatedDate = Category.CreatedDate;
                    _context.Entry(Category).CurrentValues.SetValues(vm);
                    await _context.SaveChangesAsync();

                    json.Message = "Not found";
                    json.StatusCode = 400;
                    json.Object = Category;
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
            Category Category = await _context.Category.FirstOrDefaultAsync(i => i.Id == id);
            if (Category == null)
            {
                json.StatusCode = 404;
                json.Message = "Not Found";
                json.Object = null;
                return Ok(json);
            }
            else
            {
                _context.Category.Remove(Category);
                await _context.SaveChangesAsync();
                json.StatusCode = 202;
                json.Message = "Success";
                json.Object = null;
                return Ok(json);
            }
        }
    }
}