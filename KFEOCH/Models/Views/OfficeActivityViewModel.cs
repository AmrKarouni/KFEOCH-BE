namespace KFEOCH.Models.Views
{
    public class OfficeActivityViewModel
    {
        public OfficeActivityViewModel(OfficeActivity model)
        {
            Id = model.Id;
            OfficeId = model.OfficeId;
            ActivityId = model.ActivityId;
            NameArabic = model.Activity?.NameArabic;
            NameEnglish = model.Activity?.NameEnglish;
            AddedDate = model.AddedDate;
            IsApproved = model.IsApproved;
            IsDeleted = model.IsDeleted;
        }
        public int Id { get; set; } 
        public int? OfficeId { get; set; }
        public int? ActivityId { get; set; }
        public string? NameArabic { get; set; }
        public string? NameEnglish { get; set; }
        public DateTime? AddedDate { get; set; }
        public bool IsApproved { get; set; }
        public bool IsDeleted { get; set; }
    }
}
