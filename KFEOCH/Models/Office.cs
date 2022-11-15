using KFEOCH.Models.Dictionaries;
using KFEOCH.Models.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KFEOCH.Models
{
    public class Office
    {
        public Office() { }
        public Office(OfficeRegistrationModel model)
        {
            NameArabic = model.NameArabic;
            NameEnglish = model.NameEnglish;
            TypeId = model.OfficeTypeId;
            LicenseId = model.LicenseId;
            Email = model.Email;
            PhoneNumber = model.PhoneNumber;
            IsActive = true;
        }

        public int Id { get; set; }
        [Required]
        [StringLength(100,MinimumLength =5)]
        public string? NameArabic { get; set; }
        [Required]
        [StringLength(100, MinimumLength = 5)]
        public string? NameEnglish { get; set; }
        [ForeignKey("Type")]
        public int TypeId { get; set; }
        public long LicenseId { get; set; }
        public DateTime? RegistrationDate { get; set; }
        [Required]
        [EmailAddress]
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? FaxNumber { get; set; }
        public string? MailBox { get; set; }
        public string? PostalCode { get; set; }
        public string? Address { get; set; }
        //Society of Engineers Membership Id
        public string? SEMId { get; set; }
        public bool IsVerified { get; set; }
        public bool IsActive { get; set; }
        public bool AgreeToTerms { get; set; }
        public string? LogoUrl { get; set; }
        public bool? ShowInHome { get; set; }
        public virtual OfficeType? Type { get; set; }

    }
}
