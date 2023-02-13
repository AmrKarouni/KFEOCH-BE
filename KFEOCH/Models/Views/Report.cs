namespace KFEOCH.Models.Views
{
    public class Report
    {
    }

    public class OfficeOwnerReportViewModel
    {
        public OfficeOwnerReportViewModel()
        {

        }

        public OfficeOwnerReportViewModel(OfficeOwner model)
        {
            Id = model.Id;
            OfficeId = model.OfficeId;
            OfficeNameArabic = model.Office?.NameArabic;
            OfficeNameEnglish = model.Office?.NameEnglish;
            NameArabic = model.NameArabic;
            NameEnglish = model.NameEnglish;
            PhoneNumber = model.PhoneNumber;
            PhoneNumberTwo = model.PhoneNumberTwo;
            Email = model.Email;
            GenderNameArabic = model.Gender?.NameArabic;
            GenderNameEnglish = model.Gender?.NameEnglish;
            NationalId = model.NationalId;
            NationalityNameArabic = model.Nationality?.NameArabic;
            NationalityNameEnglish = model.Nationality?.NameEnglish;
            SemId = model.SemId;
            SpecialityNameArabic = model.Speciality?.NameArabic;
            SpecialityNameEnglish = model.Speciality?.NameEnglish;
            ExperienceYears = model.ExperienceYears;
        }
        public int Id { get; set; }
        public int OfficeId { get; set; }
        public string? OfficeNameArabic { get; set; }
        public string? OfficeNameEnglish { get; set; }
        public string? NameArabic { get; set; }
        public string? NameEnglish { get; set; }
        public string? GenderNameArabic { get; set; }
        public string? GenderNameEnglish { get; set; }
        public string? NationalId { get; set; }
        public string? NationalityNameArabic { get; set; }
        public string? NationalityNameEnglish { get; set; }
        public string? SemId { get; set; }
        public string? SpecialityNameArabic { get; set; }
        public string? SpecialityNameEnglish { get; set; }
        public string? PhoneNumber { get; set; }
        public string? PhoneNumberTwo { get; set; }
        public string? Email { get; set; }
        public int? ExperienceYears { get; set; }
    }

    public class OfficeSpecialityReportViewModel
    {
        public OfficeSpecialityReportViewModel()
        {

        }

        public OfficeSpecialityReportViewModel(OfficeSpeciality model)
        {
            Id = model.Id;
            OfficeId = model.OfficeId;
            OfficeNameArbic = model.Office?.NameArabic;
            OfficeNameEnglish = model.Office?.NameEnglish;
            SpecialityId = model.SpecialityId;
            SpecialityNameArbic = model.Speciality?.NameArabic;
            SpecialityNameEnglish = model.Speciality?.NameEnglish;
            AddedDate = model.AddedDate;
        }
        public int Id { get; set; }
        public int? OfficeId { get; set; }
        public string? OfficeNameArbic { get; set; }
        public string? OfficeNameEnglish { get; set; }
        public int? SpecialityId { get; set; }
        public string? SpecialityNameArbic { get; set; }
        public string? SpecialityNameEnglish { get; set; }
        public DateTime? AddedDate { get; set; }
    }

    public class OfficeContactReportViewModel
    {
        public OfficeContactReportViewModel()
        {

        }
        public OfficeContactReportViewModel(OfficeContact model)
        {
            Id = model.Id;
            OfficeId = model.OfficeId;
            OfficeNameArbic = model.Office?.NameArabic;
            OfficeNameEnglish = model.Office?.NameEnglish;
            ContactId = model.ContactId;
            ContactNameArbic = model.Contact?.NameArabic;
            ContactNameEnglish = model.Contact?.NameEnglish;
            Contact = model.PhoneNumber;
            AddedDate = model.AddedDate;
        }
        public int Id { get; set; }
        public int? OfficeId { get; set; }
        public string? OfficeNameArbic { get; set; }
        public string? OfficeNameEnglish { get; set; }
        public int? ContactId { get; set; }
        public string? ContactNameArbic { get; set; }
        public string? ContactNameEnglish { get; set; }
        public string? Contact { get; set; }
        public DateTime? AddedDate { get; set; }
    }

    public class OfficeLicenseReportViewModel
    {
        public OfficeLicenseReportViewModel()
        {

        }

        public OfficeLicenseReportViewModel(License model)
        {
            Id = model.Id;
            OfficeId = model.OfficeId;
            LicenseId = model.Office.LicenseId;
            OfficeNameArabic = model.Office.NameArabic;
            OfficeNameEnglish = model.Office.NameEnglish;
            EntityId = model.OfficeEntityId;
            EntityNameArabic = model.OfficeEntity?.NameArabic;
            EntityNameEnglish = model.OfficeEntity?.NameEnglish;
            CreatedDate = model.CreatedDate;
            StartDate = model.StartDate;
            EndDate = model.EndDate;
            IsApproved = model.IsApproved;
            IsRejected = model.IsRejected;
            IsPending = model.IsPending;
        }
        public int Id { get; set; }
        public int OfficeId { get; set; }
        public long LicenseId { get; set; }
        public string? OfficeNameArabic { get; set; }
        public string? OfficeNameEnglish { get; set; }
        public int EntityId { get; set; }
        public string? EntityNameArabic { get; set; }
        public string? EntityNameEnglish { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsApproved { get; set; }
        public bool IsRejected { get; set; }
        public bool IsPending { get; set; }

    }
}
