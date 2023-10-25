using System.ComponentModel.DataAnnotations;

namespace LacDau.Models
{
    public class SubCategory
    {
        [Key]
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }
        public bool IsDeleted { get; set; }
        
    }
}
