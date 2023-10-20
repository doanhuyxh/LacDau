using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LacDau.Models
{
    public class PaymentNotification
    {
        [Key]
        public string Id { get; set; }
        public string PaymentRefId { get; set; }
        public DateTime Date {  get; set; }
        public double Amount { get; set; }
        public string Content { get; set; }
        public string Message { get; set; }
        public string Signature { get; set; }
        public string PaymentId { get; set; }
        public string MerchanId { get; set; }
        public string Status { get; set; }
        public string ResDate { get; set; }

        [ForeignKey("PaymentId")]
        public Payment Payment { get; set; }
    }
}
