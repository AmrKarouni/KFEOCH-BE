using KFEOCH.Models.Dictionaries;
using System.ComponentModel.DataAnnotations.Schema;

namespace KFEOCH.Models
{
    public class OfficeRequest
    {
        public OfficeRequest()
        {

        }

        public OfficeRequest(OfficeRequestBindingModel model,double amount,string paymentNumber)
        {
            Id = 0;
            OfficeId = model.OfficeId;
            RequestTypeId = model.RequestTypeId;
            CertificateEntityId = model.CertificateEntityId;
            Amount = amount;
            PaymentNumber = paymentNumber;
            ApprovedDate = CreatedDate = DoneDate = DateTime.UtcNow;
            IsPaid = IsApproved = IsDone = true;
        }
        public int Id { get; set; }
        [ForeignKey("Office")]
        public int OfficeId { get; set; }
        [ForeignKey("RequestType")]
        public int RequestTypeId { get; set; }
        [ForeignKey("CertificateEntity")]
        public int? CertificateEntityId { get; set; }
        public double Amount { get; set; }
        public bool IsPaid { get; set; } = true;
        public string? PaymentUrl { get; set; }
        public string? PaymentNumber { get; set; }
        public DateTime? CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime? CanceledDate { get; set; }
        public DateTime? ApprovedDate { get; set; }
        public DateTime? RejectedDate { get; set; }
        public DateTime? DoneDate { get; set; }
        public bool IsCanceled { get; set; } = false;
        public bool IsApproved { get; set; } = false;
        public bool IsRejected { get; set; } = false;
        public bool IsDone { get; set; } = false;
        public string? HtmlBody { get; set; }
        public virtual Office? Office { get; set; }
        public virtual RequestType? RequestType { get; set; }
        public virtual CertificateEntity? CertificateEntity { get; set; }
    }

    public class OfficeRequestBindingModel
    {
        public int OfficeId { get; set; }
        public int RequestTypeId { get; set; }
        public int? CertificateEntityId { get; set; }
        public string? Lang { get; set; }
        public string? ReturnUrl { get; set; }
    }

    public class OfficeRequestViewModel
    {
        public OfficeRequestViewModel()
        {

        }

        public OfficeRequestViewModel(OfficeRequest model, List<OfficePayment> pays)
        {
            Id = model.Id;
            OfficeId = model.OfficeId;
            OfficeLicenseId = model.Office?.LicenseId;
            OfficeNameArabic = model.Office?.NameArabic;
            OfficeNameEnglish = model.Office?.NameEnglish;

            RequestTypeId = model.RequestTypeId;
            RequestTypeNameArabic = model.RequestType?.NameArabic;
            RequestTypeNameEnglish = model.RequestType?.NameEnglish;

            CertificateEntityId = model.CertificateEntityId;
            CertificateEntityNameArabic = model.CertificateEntity?.NameArabic;
            CertificateEntityNameEnglish = model.CertificateEntity?.NameEnglish;
            Amount = model.Amount;
            IsPaid = model.IsPaid;
            PaymentUrl = model.PaymentUrl;
            PaymentNumber = model.PaymentNumber;
            CreatedDate = model.CreatedDate;
            CanceledDate = model.CanceledDate;
            ApprovedDate = model.ApprovedDate;
            RejectedDate = model.RejectedDate;
            DoneDate = model.DoneDate;
            IsCanceled = model.IsCanceled;
            IsApproved = model.IsApproved;
            IsRejected = model.IsRejected;
            IsDone = model.IsDone;
            PaymentId = pays.FirstOrDefault(x => x.PaymentNumber == model.PaymentNumber)?.Id;
            HtmlBody = model.HtmlBody;
        }

        public OfficeRequestViewModel(OfficeRequest model)
        {
            Id = model.Id;
            OfficeId = model.OfficeId;
            OfficeLicenseId = model.Office?.LicenseId;
            OfficeNameArabic = model.Office?.NameArabic;
            OfficeNameEnglish = model.Office?.NameEnglish;

            RequestTypeId = model.RequestTypeId;
            RequestTypeNameArabic = model.RequestType?.NameArabic;
            RequestTypeNameEnglish = model.RequestType?.NameEnglish;

            CertificateEntityId = model.CertificateEntityId;
            CertificateEntityNameArabic = model.CertificateEntity?.NameArabic;
            CertificateEntityNameEnglish = model.CertificateEntity?.NameEnglish;
            Amount = model.Amount;
            IsPaid = model.IsPaid;
            PaymentUrl = model.PaymentUrl;
            PaymentNumber = model.PaymentNumber;
            CreatedDate = model.CreatedDate;
            CanceledDate = model.CanceledDate;
            ApprovedDate = model.ApprovedDate;
            RejectedDate = model.RejectedDate;
            DoneDate = model.DoneDate;
            IsCanceled = model.IsCanceled;
            IsApproved = model.IsApproved;
            IsRejected = model.IsRejected;
            IsDone = model.IsDone;
        }


        public int Id { get; set; }
        public int OfficeId { get; set; }
        public long? OfficeLicenseId { get; set; }
        public string? OfficeNameArabic { get; set; }
        public string? OfficeNameEnglish { get; set; }

        public int RequestTypeId { get; set; }
        public string? RequestTypeNameArabic { get; set; }
        public string? RequestTypeNameEnglish { get; set; }

        public int? CertificateEntityId { get; set; }
        public string? CertificateEntityNameArabic { get; set; }
        public string? CertificateEntityNameEnglish { get; set; }
        public double Amount { get; set; }
        public bool IsPaid { get; set; }
        public string? PaymentUrl { get; set; }
        public string? PaymentNumber { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? CanceledDate { get; set; }
        public DateTime? ApprovedDate { get; set; }
        public DateTime? RejectedDate { get; set; }
        public DateTime? DoneDate { get; set; }
        public bool IsCanceled { get; set; }
        public bool IsApproved { get; set; }
        public bool IsRejected { get; set; }
        public bool IsDone { get; set; }
        public int? PaymentId { get; set; }
        public string HtmlBody { get; set; }
    }
}
