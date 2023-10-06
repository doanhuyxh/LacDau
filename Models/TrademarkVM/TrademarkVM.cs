using System.ComponentModel.DataAnnotations;

namespace LacDau.Models.TrademarkVM
{
    public class TrademarkVM
    {
        public int Id { get; set; }
        [Display(Name = "Tên thương hiệu")]
        public string Name { get; set; }
        public string? Logo { get; set; }
        [Display(Name="Logo")]
        public IFormFile? LogoImg { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsDeleted { get; set; }

        public static implicit operator TrademarkVM(Trademark trademark)
        {
            return new TrademarkVM
            {
                Id = trademark.Id,
                Name = trademark.Name,
                Logo = trademark.Logo,
                CreatedDate = trademark.CreatedDate,
                IsDeleted = trademark.IsDeleted,
            };
        }
        public static implicit operator Trademark(TrademarkVM vm)
        {
            return new Trademark
            {
                Id = vm.Id,
                Name = vm.Name,
                Logo = vm.Logo ?? "",
                CreatedDate = vm.CreatedDate,
                IsDeleted = vm.IsDeleted,
            };
        }
    }
}
