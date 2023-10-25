using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;

namespace LacDau.Models.SubCategoryVM
{
    public class SubCategoryVM
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        [Display(Name="Tên danh mục")]
        public string Name { get; set; }
        [Display(Name = "Slug")]
        public string Slug { get; set; }
        public bool IsDeleted { get; set; }

        public static implicit operator SubCategory(SubCategoryVM vm)
        {
            return new SubCategory
            {
                Id = vm.Id,
                CategoryId = vm.CategoryId,
                Name = vm.Name,
                Slug = vm.Slug,
                IsDeleted = vm.IsDeleted,
            };
        }
        public static implicit operator SubCategoryVM(SubCategory vm)
        {
            return new SubCategory
            {
                Id = vm.Id,
                CategoryId = vm.CategoryId,
                Name = vm.Name,
                Slug = vm.Slug,
                IsDeleted = vm.IsDeleted,
            };
        }
    }
}
