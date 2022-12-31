using KFEOCH.Models.Dictionaries;
using System.ComponentModel.DataAnnotations.Schema;

namespace KFEOCH.Models
{
    public class License
    {
        public int Id { get; set; }
        [ForeignKey("Office")]
        public int OfficeId { get; set; }
        [ForeignKey("OfficeEntity")]
        public int OfficeEntityId { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string? DocumentUrl { get; set; }
        public DateTime? UploadDate { get; set; }
        public string? Note { get; set; }
        public bool IsApproved { get; set; } = false;
        public bool IsRejected { get; set; } = false;
        public bool IsFirst { get; set; } = false;
        public bool IsPending { get; set; } = true;
        public bool IsLast { get; set; } = true;
        public virtual Office? Office { get; set; }
        public virtual OfficeEntity? OfficeEntity { get; set; }
        public virtual ICollection<Speciality>? Specialities { get; set; }
        [NotMapped]
        public bool OutDated => DateTime.UtcNow > EndDate && (IsApproved || IsPending );
    }

    public class LicenseBindingModel
    {
        public int OfficeId { get; set; }
        public int OfficeEntityId { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string? DocumentUrl { get; set; }
        public DateTime? UploadDate { get; set; }
        public string? Note { get; set; }
        public bool IsApproved { get; set; } = false;
        public bool IsRejected { get; set; } = false;
        public bool IsFirst { get; set; } = false;
        public List<int> SpecialityIds { get; set; }
    }
    public class LicenseViewModel
    {
        public LicenseViewModel()
        {

        }

        public LicenseViewModel(License model)
        {
            Id = model.Id;
            OfficeId = model.OfficeId;
            LicenseId = model.Office.LicenseId;
            OfficeNameArabic = model.Office.NameArabic;
            OfficeNameEnglish = model.Office.NameEnglish;
            EntityId = model.OfficeEntityId;
            EntityNameArabic = model.OfficeEntity.NameArabic;
            EntityNameEnglish = model.OfficeEntity.NameEnglish;
            CreatedDate = model.CreatedDate;
            StartDate = model.StartDate;
            EndDate = model.EndDate;
            DocumentUrl = model.DocumentUrl;
            UploadDate = model.UploadDate;
        }
        public int Id { get; set; }
        public int OfficeId { get; set; }
        public long LicenseId { get; set; }
        public string? OfficeNameArabic { get; set; }
        public string? OfficeNameEnglish { get; set; }
        public int EntityId { get; set; }
        public string? EntityNameArabic { get; set; }
        public string? EntityNameEnglish { get; set; }
        public DateTime CreatedDate { get; set; } 
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string? DocumentUrl { get; set; }
        public DateTime? UploadDate { get; set; }
    }
}
