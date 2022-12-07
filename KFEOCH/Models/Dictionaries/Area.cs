using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KFEOCH.Models.Dictionaries
{
    public class Area
    {
        public int Id { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 5)]
        public string? NameArabic { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 5)]
        public string? NameEnglish { get; set; }
        public string? DescriptionArabic { get; set; }
        public string? DescriptionEnglish { get; set; }
        [ForeignKey("Parent")]
        public int ParentId { get; set; }
        public bool IsDeleted { get; set; }
        public virtual Governorate? Parent { get; set; }
    }

    public class AreaViewModel : Area
    {
        public string? ParentNameArabic { get; set; }
        public string? ParentNameEnglish { get; set; }
        public AreaViewModel(Area model)
        {
            Id = model.Id;
            NameArabic = model.NameArabic;
            NameEnglish = model.NameEnglish;
            DescriptionArabic = model.DescriptionArabic;
            DescriptionEnglish = model.DescriptionEnglish;
            IsDeleted = model.IsDeleted;
            ParentId = model.ParentId;
            ParentNameArabic = model.Parent?.NameArabic;
            ParentNameEnglish = model.Parent?.NameEnglish;
        }
    }
}
