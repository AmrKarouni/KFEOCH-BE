using System.ComponentModel.DataAnnotations;

namespace KFEOCH.Models.Dictionaries
{
    public class OfficeEntity
    {
        /// <summary>
        /// مكتب هندسي
        //دار استشارية
        //شركة مهنية هندسية
        /// </summary>
        public int Id { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 5)]
        public string? NameArabic { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 5)]
        public string? NameEnglish { get; set; }
        public string? DescriptionArabic { get; set; }
        public string? DescriptionEnglish { get; set; }
        public double YearlyFees { get; set; } = 100;
        public bool IsDeleted { get; set; } = false;
    }
}
