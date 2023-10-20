using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LacDau.Models
{
    public class PaymentSignature
    {
        [Key]
        public string Id { get; set; }
        public string Value { get; set; }
        public string Algo {  get; set; }
        public DateTime Date {  get; set; }
        public string Owner { get; set; }
        public string PaymentId { get; set; }

        [ForeignKey("PaymentId")]
        public Payment Payment { get; set; }
    }
}
