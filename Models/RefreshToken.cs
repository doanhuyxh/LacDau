using System.ComponentModel.DataAnnotations;

namespace LacDau.Models
{
    public class RefreshToken
    {
        [Key]
        public int Id { get; set; }
        public string UserId { get; set; }
        public string Token { get; set; }
        public string JwtId { get; set; }
        public bool IsUsed { get; set; }
        public bool IsRevoked { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ExpirydDate { get; set;}
    }
}
