namespace KFEOCH.Models.Views
{
    public class OfficeOwnerWithDocuments
    {
        public int Id { get; set; }
        public int OfficeId { get; set; }
        public string? NameArabic { get; set; }
        public string? NameEnglish { get; set; }
        public IEnumerable<OfficeOwnerDocumentTypeView>? Documents { get; set; }
    }
    public class OfficeOwnerDocumentTypeView
    {
        public int Id { get; set; }
        public string? NameArabic { get; set; }
        public string? NameEnglish { get; set; }
        public IEnumerable<OfficeOwnerDocumentView>? Files { get; set; }
    }
    public class OfficeOwnerDocumentView
    {
        public int Id { get; set; }
        public string? DocumentUrl { get; set; }
        public DateTime? AddedDate { get; set; }
    }
}
