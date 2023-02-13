namespace KFEOCH.Models.Views
{
    public class SiteOfficeViewModel
    {
        public SiteOfficeViewModel() { }
        public SiteOfficeViewModel(Office model) 
        {
            Id = model.Id;
            NameArabic = model.NameArabic;
            NameEnglish = model.NameEnglish;
            PhoneNumber = model.PhoneNumber;
            LogoUrl = model.LogoUrl ;
            TypeArabic = model.Type?.NameArabic;
            TypeEnglish = model.Type?.NameEnglish;
            Email = model.Email;
            FaxNumber = model.FaxNumber;
            MailBox = model.MailBox;
            PostalCode = model.PostalCode;
            Address = model.Address;
        }
        public int Id { get; set; }
        public string? NameArabic { get; set; }
        public string? NameEnglish { get; set; }
        public string? PhoneNumber { get; set; }
        public string? LogoUrl { get; set; }
        public string? TypeArabic { get; set; }
        public string? TypeEnglish { get; set; }
        public string? Email { get; set; }
        public string? FaxNumber { get; set; }
        public string? MailBox { get; set; }
        public string? PostalCode { get; set; }
        public string? Address { get; set; }
    }
}
