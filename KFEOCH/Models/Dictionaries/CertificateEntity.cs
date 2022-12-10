using System.ComponentModel.DataAnnotations;

namespace KFEOCH.Models.Dictionaries
{
    public class CertificateEntity
    {
        public int Id { get; set; }
        [Required]
        [StringLength(100, MinimumLength = 5)]
        public string? NameArabic { get; set; }
        [Required]
        [StringLength(100, MinimumLength = 5)]
        public string? NameEnglish { get; set; }
        public string? DescriptionArabic { get; set; }
        public string? DescriptionEnglish { get; set; }
        public bool IsDeleted { get; set; } = false;
    }
}
