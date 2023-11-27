using LacDau.Models.CategoryVM;
using LacDau.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LacDau.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using LacDau.Services;

namespace LacDau.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme, Roles = "SuperAdmin, Admin")]
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ICommon _icommon;
        public CategoryController(ApplicationDbContext context, ICommon common)
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
            var rs = await _context.Category.Where(i => i.IsDeleted == false && i.ParentId == 0).OrderByDescending(i => i.Id).ToListAsync();
            json.Message = "Success";
            json.StatusCode = 200;
            json.Object = rs;
            return Ok(json);
        }

        [HttpGet]
        public IActionResult AddData()
        {
            CategoryVM Category = new CategoryVM();
            return PartialView("AddEditData", Category);
        }

        [HttpPost]
        public async Task<IActionResult> SaveData(CategoryVM vm)
        {
            JsonResultVM json = new JsonResultVM();
            try
            {
                if (ModelState.IsValid)
                {
                    Category Category = new Category();
                    if (vm.Id == 0)
                    {
                        if (vm.IconFile != null)
                        {
                            vm.Icon = await _icommon.UploadIconCategoryAsync(vm.IconFile);
                        }

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
                        Category = await _context.Category.FirstOrDefaultAsync(i => i.Id == vm.Id);
                        if (vm.IconFile != null)
                        {
                            Category.Icon = await _icommon.UploadIconCategoryAsync(vm.IconFile);
                        }
                        Category.Slug = vm.Slug;
                        Category.Name = vm.Name;
                        _context.Update(Category);
                        await _context.SaveChangesAsync();

                        json.Message = "Success";
                        json.StatusCode = 200;
                        json.Object = Category;
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
            CategoryVM Category = await _context.Category.FirstOrDefaultAsync(i => i.Id == id);
            return PartialView("AddEditData", Category);
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
                Category.IsDeleted = true;
                _context.Update(Category);
                await _context.SaveChangesAsync();
                json.StatusCode = 202;
                json.Message = "Success";
                json.Object = null;
                return Ok(json);
            }
        }

        public async Task<IActionResult> SubCategory(int id)
        {
            List<CategoryVM> sub = await (from c in _context.Category
                                          where c.ParentId == id && c.IsDeleted == false
                                          select new CategoryVM
                                          {
                                              IsDeleted = c.IsDeleted,
                                              Name = c.Name,
                                              Id = c.Id,
                                              Slug = c.Slug,
                                              Icon = c.Icon,
                                              CreatedDate = c.CreatedDate,
                                          }).ToListAsync();
            return PartialView("_SubCategory", sub);
        }

        public IActionResult AddEditSubCate(int id = 0)
        {
            CategoryVM vm = new CategoryVM();
            if (id != 0)
            {
                vm = _context.Category.FirstOrDefault(i => i.Id == id);
            }

            return PartialView("AddEditData", vm);
        }
    }
}
