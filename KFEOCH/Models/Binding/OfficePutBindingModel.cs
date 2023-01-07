namespace KFEOCH.Models.Binding
{
    public class OfficePutBindingModel
    {
        public int Id { get; set; }
        public string? NameArabic { get; set; }
        public string? NameEnglish { get; set; }
        public string? EmailTwo { get; set; }
        public string? PhoneNumber { get; set; }
        public string? FaxNumber { get; set; }
        public string? MailBox { get; set; }
        public string? PostalCode { get; set; }
        public string? Address { get; set; }
        public int? AreaId { get; set; }
        public int? GovernorateId { get; set; }
        public string? AutoNumberOne { get; set; }
        public string? AutoNumberTwo { get; set; }
    }
}
