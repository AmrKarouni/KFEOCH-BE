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
        public bool IsAdmin { get; set; } = false;
        public bool IsDeleted { get; set; } = false;
        public virtual ICollection<Activity> OfficeActivities { get; set; } = new List<Activity>();
        public virtual ICollection<Speciality> OfficeSpecialities { get; set; } = new List<Speciality>();
    }
}
