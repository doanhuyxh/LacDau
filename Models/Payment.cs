using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LacDau.Models
{
    public class Payment
    {
        [Key]
        public string Id { get; set; }
        public string Content { get; set; }
        public string Currentcy { get; set; }
        public string RefId { get; set; }
        public double RequireAmount { get; set; }
        public DateTime PayDate { get; set; }
        public DateTime ExpireDate { get; set; }
        public string Language { get; set; }
        public string MerchanId { get; set; }
        public string DestinationId { get; set; }
        public double PayAmount { get; set; }
        [MaxLength(30)]
        public string Sattus { get; set; }
        public string LastMessage { get; set; }

        [ForeignKey("MerchanId")]
        public Merchant Merchant { get; set; }
        [ForeignKey("DestinationId")]
        public PaymentDestination PaymentDestination { get; set; }
    }
}
