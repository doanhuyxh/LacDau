using System.ComponentModel.DataAnnotations;

namespace LacDau.Models
{
    public class Merchant
    {
        [Key]
        public string Id { get; set; }
        public string Name { get; set; }
        public string WebLink { get; set; }
        public string IpnUrl { get; set; }
        public string ReturnUrl { get; set; }
        public string SecreKey { get; set; }
        public bool IsActive { get; set; }
        public string? CreatedBy { get; set; } = string.Empty;
        public DateTime? CreatedAt { get; set; }
        public string? LastUpdatedByy { get; set; }
        public DateTime? LastUpdatedAt { get; set; }
    }
}
