namespace KFEOCH.Models.Views
{
    public class FilterModel
    {
        public string? SearchQuery { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public string? Sort { get; set; }
        public string? Order { get; set; }
        public List<int>? Types { get; set; }
        public List<int>? Entities { get; set; }
        public List<int>? LegalEntities { get; set; }
        public List<int>? Specialities { get; set; }
        public List<int>? Activities { get; set; }
        public bool? IsVerified { get; set; }
        public bool? IsActive { get; set; }
        public List<int>? OfficeTypes { get; set; }
        public List<int>? Offices { get; set; }
        public string? PaymentCategoryEnglish { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
    }
 }
