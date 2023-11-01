using LacDau.Data;
using LacDau.Models.CategoryVM;
using LacDau.Models.SubCategoryVM;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LacDau.ViewComponents
{
    public class CategoriesViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;

        public CategoriesViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            List<ListCategory> listcate = new List<ListCategory>();
            listcate = await (from cate in _context.Category
                              where cate.IsDeleted == false
                              select new ListCategory { 
                                  Name = cate.Name,
                                  Slug = cate.Slug,
                                  Icon = cate.Icon,
                                  ListSubCategory = _context.SubCategory.Where(sub=>sub.CategoryId == cate.Id && sub.IsDeleted == false).ToList(),
                              }).ToListAsync();

            return View(listcate);
        }
    }
}
