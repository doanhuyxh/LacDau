using LacDau.Models.CategoryVM;
using LacDau.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LacDau.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.Cookies;
using LacDau.Models.SubCategoryVM;

namespace LacDau.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme, Roles = "SuperAdmin, Admin")]
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _context;
        public CategoryController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> GetData()
        {
            JsonResultVM json = new JsonResultVM();
            var rs = await _context.Category.Where(i => i.IsDeleted == false).OrderByDescending(i => i.Id).ToListAsync();
            json.Message = "Success";
            json.StatusCode = 200;
            json.Object = rs;
            return Ok(json);
        }

        [HttpGet]
        public IActionResult AddData()
        {
            CategoryVM Category = new CategoryVM();
            return View(Category);
        }

        [HttpPost]
        public async Task<IActionResult> AddData(CategoryVM vm)
        {
            JsonResultVM json = new JsonResultVM();
            try
            {
                if (ModelState.IsValid)
                {
                    Category Category = new Category();
                    if (vm.Id == 0)
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
                        Category = await _context.Category.FirstOrDefaultAsync(i => i.Id == vm.Id);
                        vm.CreatedDate = Category.CreatedDate;
                        _context.Entry(Category).CurrentValues.SetValues(vm);
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
            return View(Category);
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

        public IActionResult SubCategory(int id)
        {
            List<SubCategoryVM> sub = (from c in _context.SubCategory
                                       where c.CategoryId == id && c.IsDeleted == false
                                       select new SubCategoryVM
                                       {
                                           IsDeleted = c.IsDeleted,
                                           Name = c.Name,
                                           CategoryId = c.CategoryId,
                                           Id = c.Id,
                                           Slug = c.Slug,
                                       }).ToList();
            ViewBag.SubCategory = sub;
            return PartialView("_SubCategory");
        }

        public IActionResult AddSubCate()
        {
            SubCategoryVM sub = new SubCategoryVM();
            return PartialView("_AddEditSubCate", sub);
        }
        public IActionResult EditSubCate(int id)
        {
            SubCategoryVM sub = _context.SubCategory.FirstOrDefault(c => c.Id == id);

            return PartialView("_AddEditSubCate", sub);
        }

        [HttpPost]
        public async Task<IActionResult> SaveSub(SubCategoryVM sub)
        {
            JsonResultVM json = new JsonResultVM();
            try
            {
                SubCategory subCate = new SubCategory();
                if (sub.Id == 0)
                {
                    subCate = sub;
                    subCate.IsDeleted = false;
                    await _context.AddAsync(subCate);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    subCate = await _context.SubCategory.FirstOrDefaultAsync(i => i.Id == sub.Id);
                    subCate.Name = sub.Name;
                    subCate.Slug = sub.Slug;
                    _context.Update(subCate);
                    await _context.SaveChangesAsync();
                }
                json.Object = subCate;
                json.Message = "Succes";
                json.StatusCode = 201;
                return Json(json);
            }
            catch (Exception ex)
            {
                json.Object = ex;
                json.Message = "error";
                json.StatusCode = 500;
                return BadRequest(json);
            }
        }

        public async Task<IActionResult> DeleteSubCate(int id)
        {
            JsonResultVM json = new JsonResultVM();
            SubCategory subCategory = await _context.SubCategory.FirstOrDefaultAsync(i => i.Id == id);
            if (subCategory == null)
            {
                json.StatusCode = 404;
                json.Message = "Not Found";
                json.Object = null;
                return Ok(json);
            }
            else
            {
                _context.SubCategory.Remove(subCategory);
                await _context.SaveChangesAsync();
                json.StatusCode = 202;
                json.Message = "Success";
                json.Object = null;
                return Ok(json);
            }
        }
    }
}
