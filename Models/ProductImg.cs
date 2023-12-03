using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LacDau.Models
{
    public class ProductImg
    {
        [Key]
        public int Id { get; set; }
        public string ImgPath { get; set; }
        public bool IsDelete { get; set; }
        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }
    }
}
