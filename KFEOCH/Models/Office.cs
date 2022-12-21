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
            RegistrationDate = DateTime.UtcNow;
            IsLocal = null;
            IsActive = true;
        }
        public Office(Office model)
        {
            NameArabic = model.NameArabic;
            NameEnglish = model.NameEnglish;
            EstablishmentDate = model.EstablishmentDate;
            LicenseEndDate = model.LicenseEndDate;
            Email = model.Email;
            EmailTwo = model.EmailTwo;
            PhoneNumber = model.PhoneNumber;
            FaxNumber = model.FaxNumber;
            MailBox = model.MailBox;
            PostalCode = model.PostalCode;
            Address = model.Address;
            AreaId = model.AreaId;
            EntityId = model.EntityId;
            LegalEntityId = model.LegalEntityId;
            AgreeToTerms = model.AgreeToTerms;
            LogoUrl = model.LogoUrl;
            ShowInHome = model.ShowInHome;
            AutoNumberOne = model.AutoNumberOne;
            AutoNumberTwo = model.AutoNumberTwo;
            IsLocal = model.Type?.IsLocal;
        }

        public int Id { get; set; }
        [Required]
        [StringLength(100, MinimumLength = 5)]
        public string? NameArabic { get; set; }
        [Required]
        [StringLength(100, MinimumLength = 5)]
        public string? NameEnglish { get; set; }
        [ForeignKey("Type")]
        public int TypeId { get; set; }
        public long LicenseId { get; set; }
        public DateTime? RegistrationDate { get; set; }
        public DateTime? EstablishmentDate { get; set; }
        public DateTime? LicenseEndDate { get; set; }
        [Required]
        [EmailAddress]
        public string? Email { get; set; }
        [EmailAddress]
        public string? EmailTwo { get; set; }
        public string? PhoneNumber { get; set; }
        public string? FaxNumber { get; set; }
        public string? MailBox { get; set; }
        public string? PostalCode { get; set; }
        public string? Address { get; set; }
        [ForeignKey("Area")]
        public int? AreaId { get; set; }
        [ForeignKey("Governorate")]
        public int? GovernorateId { get; set; }
        [ForeignKey("Country")]
        public int? CountryId { get; set; }
        [ForeignKey("Entity")]
        public int? EntityId { get; set; }
        [ForeignKey("LegalEntity")]
        public int? LegalEntityId { get; set; }
        public bool IsVerified { get; set; }
        public bool IsActive { get; set; }
        public bool AgreeToTerms { get; set; }
        public string? LogoUrl { get; set; }
        public bool? ShowInHome { get; set; }
        public string? AutoNumberOne{ get; set; }
        public string? AutoNumberTwo { get; set; }
        public bool? IsLocal { get; set; }
        public virtual OfficeType? Type { get; set; }
        public virtual Area? Area { get; set; }
        public virtual Governorate? Governorate { get; set; }
        public virtual Country? Country { get; set; }
        public virtual OfficeEntity? Entity { get; set; }
        public virtual OfficeLegalEntity? LegalEntity { get; set; }
        public virtual ICollection<OfficeDocument>? Documents { get; set; }
    }
}
