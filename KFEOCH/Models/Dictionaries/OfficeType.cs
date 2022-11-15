using System.ComponentModel.DataAnnotations;

namespace KFEOCH.Models.Dictionaries
{
    public class OfficeType
    {
        /// <summary>
        /// Office Type (local - foerign - Const or Admin as hidden)
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
        public bool IsAdmin { get; set; }
        public bool IsDeleted { get; set; }
        public virtual ICollection<OfficeActivity> OfficeActivities { get; set; } = new List<OfficeActivity>();
        public virtual ICollection<OfficeSpeciality> OfficeSpecialities { get; set; } = new List<OfficeSpeciality>();
    }
}
