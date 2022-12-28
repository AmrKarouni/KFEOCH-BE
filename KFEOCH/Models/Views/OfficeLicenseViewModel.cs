namespace KFEOCH.Models.Views
{
    public class OfficeLicenseViewModel
    {
        public OfficeLicenseViewModel()
        {

        }

        public OfficeLicenseViewModel(OfficeLicense model)
        {
            Id = model.Id;
            OfficeId = model.OfficeId;
            OfficeNameArabic = model.Office.NameArabic;
            OfficeNameEnglish = model.Office.NameEnglish;
            PaymentTypeNameArabic = model.PaymentType?.NameArabic;
            PaymentTypeNameEnglish = model.PaymentType?.NameEnglish;
            CreateDate = model.CreateDate;
            RegistrationStartDate = model.RegistrationStartDate;
            RegistrationEndDate = model.RegistrationEndDate;
            HasDocument = string.IsNullOrEmpty(model.DocumentUrl) ? false : true ;
            PaymentAmount = model.PaymentAmount;
            PaymentNumber = model.PaymentNumber;
            IsPaid = model.IsPaid;
            IsApproved = model.IsApproved;
            IsCanceled = model.IsCanceled;
            IsRejected = model.IsRejected;
        }


        public int Id { get; set; }
        public int OfficeId { get; set; }
        public string? OfficeNameArabic { get; set; }
        public string? OfficeNameEnglish { get; set; }
        public string? PaymentTypeNameArabic { get; set; }
        public string? PaymentTypeNameEnglish { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? RegistrationStartDate { get; set; }
        public DateTime? RegistrationEndDate { get; set; }
        public bool? HasDocument { get; set; }
        public double? PaymentAmount { get; set; }
        public string? PaymentNumber { get; set; }
        public bool? IsPaid { get; set; }
        public bool? IsApproved { get; set; } = false;
        public bool? IsRejected { get; set; } = false;
        public bool? IsCanceled { get; set; } = false;
    }
}
