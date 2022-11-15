using System.ComponentModel.DataAnnotations;

namespace KFEOCH.Models.Dictionaries
{
    public class OfficeLegalEntity
    {
        /// <summary>
        /// فردية
        ///تضامنية
        ///شركة
        /// </summary>
        public int Id { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 4)]
        public string? NameArabic { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 4)]
        public string? NameEnglish { get; set; }
        public string? DescriptionArabic { get; set; }
        public string? DescriptionEnglish { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
