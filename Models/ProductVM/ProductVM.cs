namespace LacDau.Models.ProductVM
{
    public class ProductVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string? Img1 { get; set; }
        public string? Img2 { get; set; }
        public string? Video { get; set; }
        public int TrademarkId { get; set; }
        public int CategoryId { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsHome { get; set; }

        public IFormFile? Img1File { get; set; }
        public IFormFile? Img2File { get; set; }
        public IFormFile? VideoFile { get; set; }

        public static implicit operator ProductVM(Product product)
        {
            return new ProductVM
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Img1 = product.Img1,
                Img2 = product.Img2,
                Video = product.Video,
                TrademarkId = product.TrademarkId,
                CategoryId = product.CategoryId,
                IsHome = product.IsHome,
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
                Description = vm.Description,
                Img1 = vm.Img1 ?? "",
                Img2 = vm.Img2 ?? "",
                Video = vm.Video ?? "",
                TrademarkId = vm.TrademarkId,
                IsHome = vm.IsHome,
                CategoryId = vm.CategoryId,
                CreatedDate = vm.CreatedDate,
                IsDeleted = vm.IsDeleted,
            };
        }
    }
}
