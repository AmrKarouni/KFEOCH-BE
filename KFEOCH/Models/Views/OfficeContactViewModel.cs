namespace KFEOCH.Models.Views
{
    public class OfficeContactViewModel
    {
        public OfficeContactViewModel(OfficeContact model)
        {
            Id = model.Id;
            OfficeId = model.OfficeId;
            ContactId = model.ContactId;
            ContactNameArabic = model.Contact?.NameArabic;
            ContactNameEnglish = model.Contact?.NameEnglish;
            PhoneNumber = model.PhoneNumber;
            AddedDate = model.AddedDate;
            IsApproved = model.IsApproved;
            IsDeleted = model.IsDeleted;
        }
        public int Id { get; set; }
        public int? OfficeId { get; set; }
        public int? ContactId { get; set; }
        public string? ContactNameArabic { get; set; }
        public string? ContactNameEnglish { get; set; }
        public string? PhoneNumber { get; set; }
        public DateTime? AddedDate { get; set; }
        public bool IsApproved { get; set; }
        public bool IsDeleted { get; set; }
    }
}
