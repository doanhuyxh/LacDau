using System.ComponentModel.DataAnnotations;

namespace LacDau.Models
{
    public class PaymentDestination
    {
        [Key]
        public string Id { get; set; }
        public string Logo { get; set; }
        public string ShortName { get; set; }
        public string Name { get; set; }
        public int SortIndex { get; set; }
        public string ParentId { get; set; }
        public bool IsActive { get; set; }
        public string? CreatedBy { get; set; } = string.Empty;
        public DateTime? CreatedAt { get; set; }
        public string? LastUpdatedBy { get; set; }
        public DateTime? LastUpdatedAt { get; set; }

    }
}
