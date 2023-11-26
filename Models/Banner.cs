using System.ComponentModel.DataAnnotations;

namespace LacDau.Models
{
    public class Banner
    {
        [Key]
        public int Id { get; set; }
        public string Content { get; set; }
        public int Type { get; set; } //  Type = 1 is Image banner Main, Type = 2 is banner right, Type = 3 is banner bottom, Type = 4 is banner text
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsDelete { get; set; }
    }
}
