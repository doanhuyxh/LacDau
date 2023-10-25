namespace LacDau.Models.CategoryVM
{
    public class CategoryVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsDeleted { get; set; }

        public static implicit operator CategoryVM(Category category)
        {
            return new CategoryVM
            {
                Id = category.Id,
                Name = category.Name,
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
                Slug = vm.Slug,
                CreatedDate = vm.CreatedDate,
                IsDeleted = vm.IsDeleted,
            };
        }
    }
}
