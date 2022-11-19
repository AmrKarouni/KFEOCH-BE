using KFEOCH.Models.Dictionaries;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KFEOCH.Models
{
    public class OfficeOwner
    {
        public OfficeOwner() { }
        public int Id { get; set; }
        [ForeignKey("Office")]
        public int OfficeId { get; set; }
        [Required]
        [StringLength(100, MinimumLength = 5)]
        public string? NameArabic { get; set; }
        [Required]
        [StringLength(100, MinimumLength = 5)]
        public string? NameEnglish { get; set; }
        [ForeignKey("Gender")]
        public int GenderId { get; set; }
        public string? NationalId { get; set; }
        public string? SemId { get; set; }
        [ForeignKey("Speciality")]
        public int? SpecialityId { get; set; }
        public int? ExperienceYears { get; set; }
        public string? SignatureUrl { get; set; }
        public string? CvUrl { get; set; }
        public string? CertificateUrl { get; set; }
        public bool IsApproved { get; set; }
        public bool IsDeleted { get; set; }
        public virtual Office? Office { get; set; }
        public virtual Gender? Gender { get; set; }
        public virtual OfficeOwnerSpeciality? Speciality { get; set; }
    }
}
