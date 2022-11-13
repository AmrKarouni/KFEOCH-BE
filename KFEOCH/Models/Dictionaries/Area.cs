using System.ComponentModel.DataAnnotations.Schema;

namespace KFEOCH.Models.Dictionaries
{
    public class Area
    {
        public int Id { get; set; }
        public string? NameArabic { get; set; }
        public string? NameEnglish { get; set; }
        public string? DescriptionArabic { get; set; }
        public string? DescriptionEnglish { get; set; }
        [ForeignKey("Governorate")]
        public int GovernorateId { get; set; }
        public bool IsDeleted { get; set; }
        public virtual Governorate? Governorate { get; set; }
    }
}
