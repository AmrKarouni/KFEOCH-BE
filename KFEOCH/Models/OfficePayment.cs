using KFEOCH.Models.Dictionaries;
using System.ComponentModel.DataAnnotations.Schema;

namespace KFEOCH.Models
{
    public class OfficePayment
    {
        public int Id { get; set; }
        [ForeignKey("Type")]
        public int TypeId { get; set; }
        [ForeignKey("Request")]
        public int RequestId { get; set; }
        public DateTime PaymentDate { get; set; }
        public double Amount { get; set; }
        public bool IsPaid { get; set; } = true;
        public string? PaymentUrl { get; set; }
        public string? PaymentId { get; set; }
        public virtual PaymentType? Type { get; set; }
        public virtual OfficeRequest? Request { get; set; }
    }
}
