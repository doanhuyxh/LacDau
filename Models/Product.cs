using System.ComponentModel.DataAnnotations;

namespace LacDau.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }
        public string Description { get; set; }
        public string Video { get; set; } = string.Empty;
        public int TrademarkId { get; set; }
        public int CategoryId { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsHome { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public string Price { get; set; }
        public virtual ICollection<ProductImg> ProductImg { get; set; }
    }
}
