namespace KFEOCH.Models.Views
{
    public class OfficeWithDocuments
    {
        public int Id { get; set; }
        public string? NameArabic { get; set; }
        public string? NameEnglish { get; set; }
        public IEnumerable<OfficeDocumentTypeView>? Documents { get; set; }
    }
    public class OfficeDocumentTypeView
    {
        public int Id { get; set; }
        public string? NameArabic { get; set; }
        public string? NameEnglish { get; set; }
        public IEnumerable<OfficeDocumentView>? Files { get; set; }
    }
    public class OfficeDocumentView
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? DocumentUrl { get; set; }
        public DateTime? AddedDate { get; set; }
    }
}
