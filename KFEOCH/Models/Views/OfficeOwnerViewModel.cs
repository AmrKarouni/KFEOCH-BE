namespace KFEOCH.Models.Views
{
    public class OfficeOwnerViewModel
    {
        public OfficeOwnerViewModel(OfficeOwner model)
        {
            Id= model.Id;
            OfficeId= model.OfficeId;
            NameArabic= model.NameArabic;
            NameEnglish= model.NameEnglish;
            GenderId= model.GenderId;
            GenderNameArabic = model.Gender?.NameArabic;
            GenderNameEnglish = model.Gender?.NameEnglish;
            NationalId = model.NationalId;
            SemId = model.SemId;
            SpecialityId = model.SpecialityId;
            SpecialityNameArabic = model.Speciality?.NameArabic;
            SpecialityNameEnglish = model.Speciality?.NameEnglish;
            ExperienceYears = model.ExperienceYears;
            PositionNameArabic = model.Position?.NameArabic;
            PositionNameEnglish = model.Position?.NameEnglish;
            IsApproved = model.IsApproved;
            IsDeleted = model.IsDeleted;
        }
        public int Id { get; set; }
        public int OfficeId { get; set; }
        public string? NameArabic { get; set; }
        public string? NameEnglish { get; set; }
        public int GenderId { get; set; }
        public string? GenderNameArabic { get; set; }
        public string? GenderNameEnglish { get; set; }
        public string? NationalId { get; set; }
        public string? SemId { get; set; }
        public int? SpecialityId { get; set; }
        public string? SpecialityNameArabic { get; set; }
        public string? SpecialityNameEnglish { get; set; }
        public int? ExperienceYears { get; set; }
        public string? PositionNameArabic { get; set; }
        public string? PositionNameEnglish { get; set; }
        public bool IsApproved { get; set; }
        public bool IsDeleted { get; set; }
    }
}
