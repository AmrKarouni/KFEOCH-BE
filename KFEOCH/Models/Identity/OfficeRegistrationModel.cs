namespace KFEOCH.Models.Identity
{
    public class OfficeRegistrationModel
    {
        /// <summary>
        /// Office Registration Binding Model
        /// missing data annotations
        /// </summary>
        public long LicenseId { get; set; }
        public int OfficeTypeId { get; set; }
        public string? NameArabic { get; set; }
        public string? NameEnglish { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Password { get; set; }

    }
}
