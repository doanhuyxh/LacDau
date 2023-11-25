using System.ComponentModel.DataAnnotations;

namespace LacDau.Models.CategoryVM
{
    public class CategoryVM
    {
        public int Id { get; set; }
        public int ParentId { get; set; }
        [Display(Name = "Tên loại")]
        public string Name { get; set; }
        public string Slug { get; set; }
        public string? Icon { get; set; }
        [Display(Name = "ảnh icon")]
        public IFormFile? IconFile { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsDeleted { get; set; }

        public static implicit operator CategoryVM(Category category)
        {
            return new CategoryVM
            {
                Id = category.Id,
                ParentId = category.ParentId,
                Name = category.Name,
                Icon = category.Icon,
                CreatedDate = category.CreatedDate,
                Slug = category.Slug,
                IsDeleted = category.IsDeleted,
            };
        }

        public static implicit operator Category(CategoryVM vm)
        {
            return new Category
            {
                Id = vm.Id,
                Name = vm.Name,
                ParentId = vm.ParentId,
                Icon =vm.Icon??"",
                Slug = vm.Slug,
                CreatedDate = vm.CreatedDate,
                IsDeleted = vm.IsDeleted,
            };
        }
    }
}
