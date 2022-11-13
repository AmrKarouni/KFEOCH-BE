namespace KFEOCH.Models.Views
{
    public class SiteOfficeViewModel
    {
        public SiteOfficeViewModel() { }
        public SiteOfficeViewModel(Office model) 
        {
            NameArabic = model.NameArabic;
            NameEnglish = model.NameEnglish;
            PhoneNumber = model.PhoneNumber;
            LogoUrl = model.LogoUrl;
            TypeArabic = model.Type?.NameArabic;
            TypeEnglish = model.Type?.NameEnglish;
        }
        public string? NameArabic { get; set; }
        public string? NameEnglish { get; set; }
        public string? PhoneNumber { get; set; }
        public string? LogoUrl { get; set; }
        public string? TypeArabic { get; set; }
        public string? TypeEnglish { get; set; }
    }
}
