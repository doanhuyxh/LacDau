using System.ComponentModel.DataAnnotations;

namespace LacDau.Models
{
    public class Trademark
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Logo { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsDeleted { get; set; }
    }
}
