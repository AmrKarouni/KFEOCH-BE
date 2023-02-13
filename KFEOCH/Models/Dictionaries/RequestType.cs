using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KFEOCH.Models.Dictionaries
{
    public class RequestType
    {
        public int Id { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string? NameArabic { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string? NameEnglish { get; set; }
        public string? DescriptionArabic { get; set; }
        public string? DescriptionEnglish { get; set; }
        public double Amount { get; set; }
        public bool IsDeleted { get; set; } = false;
    }
}
