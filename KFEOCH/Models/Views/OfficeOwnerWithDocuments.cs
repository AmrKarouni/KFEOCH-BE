namespace KFEOCH.Models.Views
{
    public class OfficeOwnerWithDocuments
    {
        public int Id { get; set; }
        public int OfficeId { get; set; }
        public string? NameArabic { get; set; }
        public string? NameEnglish { get; set; }
        public int GenderId { get; set; }
        public string? NationalId { get; set; }
        public int? NationalityId { get; set; }
        public int? PositionId { get; set; }
        public string? PhoneNumber { get; set; }
        public string? PhoneNumberTwo { get; set; } 
        public string? Email { get; set; }
        public string? SemId { get; set; }
        public int? SpecialityId { get; set; }
        public int? ExperienceYears { get; set; }
        public bool IsApproved { get; set; }
        public bool IsDeleted { get; set; }
        public IEnumerable<OfficeOwnerDocumentTypeView>? Documents { get; set; }
    }
    public class OfficeOwnerDocumentTypeView
    {
        public int Id { get; set; }
        public string? NameArabic { get; set; }
        public string? NameEnglish { get; set; }
        public bool? HasForm { get; set; }
        public string? FormUrl { get; set; }
        public IEnumerable<OfficeOwnerDocumentView>? Files { get; set; }
    }
    public class OfficeOwnerDocumentView
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? DocumentUrl { get; set; }
        public DateTime? AddedDate { get; set; }
    }
}
