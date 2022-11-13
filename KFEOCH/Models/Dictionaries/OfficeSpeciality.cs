using System.ComponentModel.DataAnnotations.Schema;

namespace KFEOCH.Models.Dictionaries
{
    public class OfficeSpeciality
    {
        public int Id { get; set; }
        public string? NameArabic { get; set; }
        public string? NameEnglish { get; set; }
        public string? DescriptionArabic { get; set; }
        public string? DescriptionEnglish { get; set; }
        [ForeignKey("OfficeType")]
        public int OfficeTypeId { get; set; }
        public bool IsDeleted { get; set; }
        public virtual OfficeType? OfficeType { get; set; }
    }
}
