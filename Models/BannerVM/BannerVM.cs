using System.ComponentModel.DataAnnotations;

namespace LacDau.Models.BannerVM
{
    public class BannerVM
    {
        public int Id { get; set; }
        public string? Content { get; set; }
        [Display(Name ="Loại banner")]
        public int Type { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsDelete { get; set; }
        [Display(Name = "Ảnh")]
        public IFormFile? ContentFile { get; set; }

        public static implicit operator Banner(BannerVM vm)
        {
            return new Banner
            {
                Id = vm.Id,
                Content = vm.Content??"",
                Type = vm.Type,
                IsActive = vm.IsActive,
                CreatedDate = vm.CreatedDate,
                IsDelete = vm.IsDelete,
            };
        }
        public static implicit operator BannerVM(Banner vm)
        {
            return new BannerVM
            {
                Id = vm.Id,
                Content = vm.Content,
                Type = vm.Type,
                IsActive = vm.IsActive,
                CreatedDate = vm.CreatedDate,
                IsDelete = vm.IsDelete,
            };
        }
    }
}
