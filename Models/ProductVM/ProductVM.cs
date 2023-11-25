using System.ComponentModel.DataAnnotations;

namespace LacDau.Models.ProductVM
{
    public class ProductVM
    {
        public int Id { get; set; }
        [Display(Name = "Tên sản phẩm")]
        public string Name { get; set; }
        public string Slug { get; set; }
        [Display(Name = "Mô tả")]
        public string Description { get; set; }
        public string? Video { get; set; }
        [Display(Name = "Thương hiệu")]
        public int TrademarkId { get; set; }
        public string? TrademarkName { get; set; }
        [Display(Name = "Danh mục")]
        public int CategoryId { get; set; }
        [Display(Name = "Giá")]
        public string Price { get; set; }
        public string? CategoryName { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsHome { get; set; }
        public bool IsActive { get; set; }
        [Display(Name = "Video")]
        public IFormFile? VideoFile { get; set; }
        public List<ProductImg>? ProductImg { get; set; }

        public static implicit operator ProductVM(Product product)
        {
            return new ProductVM
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                Slug = product.Slug,
                Description = product.Description,
                Video = product.Video,
                TrademarkId = product.TrademarkId,
                CategoryId = product.CategoryId,
                IsHome = product.IsHome,
                IsActive = product.IsActive,
                CreatedDate = product.CreatedDate,
                IsDeleted = product.IsDeleted,
            };
        }
        public static implicit operator Product(ProductVM vm)
        {
            return new Product
            {
                Id = vm.Id,
                Name = vm.Name,
                Price = vm.Price,
                Slug = vm.Slug,
                Description = vm.Description,
                Video = vm.Video ?? "",
                TrademarkId = vm.TrademarkId,
                IsHome = vm.IsHome,
                IsActive = vm.IsActive,
                CategoryId = vm.CategoryId,
                CreatedDate = vm.CreatedDate,
                IsDeleted = vm.IsDeleted,
            };
        }
    }
}
