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
        public double YearsCount { get; set; } = 1;
        public bool IsPaid { get; set; } = true;
        public string? PaymentUrl { get; set; }
        public string? PaymentNumber { get; set; }
        public DateTime? MembershipEndDate { get; set; }
        public virtual Office? Office { get; set; }
        public virtual PaymentType? Type { get; set; }
    }

    public class OfficePaymentViewModel
    {
        public string? StatusNameArabic { get; set; }
        public string? StatusNameEnglish { get; set; }
        public DateTime? CurrentMembershipEndDate { get; set; }
        public DateTime? NextMembershipEndDate { get; set; }
        public double TotalAmount { get; set; } 
        public List<OfficePayment>? Payments { get; set; }
    }

    public class OfficePaymentWithTypeViewModel
    {
        public OfficePaymentWithTypeViewModel()
        {

        }

        public OfficePaymentWithTypeViewModel(OfficePayment model)
        {
            Id = model.Id ;
            TypeId = model.TypeId ;
            PaymentTypeNameArabic = model.Type.NameArabic ;
            PaymentTypeNameEnglish = model.Type.NameEnglish ;
            OfficeId = model.OfficeId ;
            RequestNameArabic = model.RequestNameArabic ;
            RequestNameEnglish = model.RequestNameEnglish ;
            PaymentDate = model.PaymentDate ;
            Amount = model.Amount ;
            YearsCount = model.YearsCount ;
            IsPaid = model.IsPaid ;
            PaymentUrl = model.PaymentUrl ;
            PaymentNumber = model.PaymentNumber ;
            MembershipEndDate = model.MembershipEndDate ;
        }

        public int Id { get; set; }
        public int TypeId { get; set; }
        public string? PaymentTypeNameArabic { get; set; }
        public string? PaymentTypeNameEnglish { get; set; }
        public int OfficeId { get; set; }
        public string? RequestNameArabic { get; set; }
        public string? RequestNameEnglish { get; set; }
        public DateTime PaymentDate { get; set; }
        public double Amount { get; set; }
        public double YearsCount { get; set; }
        public bool IsPaid { get; set; }
        public string? PaymentUrl { get; set; }
        public string? PaymentNumber { get; set; }
        public DateTime? MembershipEndDate { get; set; }
    }
}
