using KFEOCH.Models.Dictionaries;
using System.ComponentModel.DataAnnotations.Schema;

namespace KFEOCH.Models
{
    public class OfficePayment
    {
        public int Id { get; set; }
        [ForeignKey("Type")]
        public int TypeId { get; set; }
        [ForeignKey("Office")]
        public int OfficeId { get; set; }
        public string? RequestNameArabic { get; set; }
        public string? RequestNameEnglish { get; set; }
        public DateTime PaymentDate { get; set; }
        public double Amount { get; set; }
        public bool IsPaid { get; set; } = true;
        public string? PaymentUrl { get; set; }
        public string? PaymentNumber { get; set; }
        public virtual Office? Office { get; set; }
    }

    public class OfficePaymentViewModel
    {
        public string? StatusNameArabic { get; set; }
        public string? StatusNameEnglish { get; set; }
        public DateTime? EndDate { get; set; }
        public List<OfficePayment>? Payments { get; set; }  
    }
}
