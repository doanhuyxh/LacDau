using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LacDau.Models
{
    public class PaymentTransaction
    {
        [Key]
        public string Id { get; set; }
        public string Message { get; set; }
        public string Payload {set; get;}
        [MaxLength(30)]
        public string Status { get; set; }
        public double Amout { get; set; }
        public DateTime Date { get; set; }
        public string PaymentId { get; set; }
        public string TranRefId { get; set; }

        [ForeignKey("PaymentId")]
        public Payment Payment { get; set; }
    }
}
