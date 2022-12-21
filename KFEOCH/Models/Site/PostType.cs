using System.ComponentModel.DataAnnotations;

namespace KFEOCH.Models.Site
{
    public class PostType
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
        public string? Url { get; set; }
        public bool IsDeleted { get; set; } = false;
        public virtual ICollection<Post>? Posts { get; set; }
    }
}
