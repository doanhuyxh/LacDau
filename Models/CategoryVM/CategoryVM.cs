namespace LacDau.Models.CategoryVM
{
    public class CategoryVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsDeleted { get; set; }

        public static implicit operator CategoryVM(Category category)
        {
            return new CategoryVM
            {
                Id = category.Id,
                Name = category.Name,
                CreatedDate = category.CreatedDate,
                IsDeleted = category.IsDeleted,
            };
        }

        public static implicit operator Category(CategoryVM vm)
        {
            return new Category
            {
                Id = vm.Id,
                Name = vm.Name,
                CreatedDate = vm.CreatedDate,
                IsDeleted = vm.IsDeleted,
            };
        }
    }
}
