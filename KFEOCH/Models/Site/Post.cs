using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KFEOCH.Models.Site
{
    public class Post
    {
        public  Post()
        {
        }
        public Post(PostBindingModel model)
        {
            TitleArabic = model.TitleArabic;
            TitleEnglish = model.TitleEnglish;
            SubTitleArabic = model.SubTitleArabic;
            SubTitleEnglish = model.SubTitleEnglish;
            BodyArabic = model.BodyArabic;
            BodyEnglish = model.BodyEnglish;
            TypeId = model.TypeId;
            IsPublished = model.IsPublished;
        }
        public int Id { get; set; }
        [Required]
        [StringLength(100, MinimumLength = 5)]
        public string? TitleArabic { get; set; }
        [Required]
        [StringLength(100, MinimumLength = 5)]
        public string? TitleEnglish { get; set; }
        public string? SubTitleArabic { get; set; }
        public string? SubTitleEnglish { get; set; }
        public string? BodyArabic { get; set; }
        public string? BodyEnglish { get; set; }
        [ForeignKey("Type")]
        public int TypeId { get; set; }
        public string? ImageUrl { get; set; }
        public string? thumbnailUrl { get; set; }
        public DateTime? CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime? PublishDate { get; set; } = DateTime.UtcNow;
        public string? Url { get; set; }
        public bool IsPublished { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
        public virtual PostType? Type { get; set; }
        public virtual ICollection<Section>? Sections { get; set; }
    }

    public class Section
    {
        public int Id { get; set; }
        [Required]
        [StringLength(100, MinimumLength = 5)]
        public string? TitleArabic { get; set; }
        [Required]
        [StringLength(100, MinimumLength = 5)]
        public string? TitleEnglish { get; set; }
        public string? BodyArabic { get; set; }
        public string? BodyEnglish { get; set; }
        [ForeignKey("Post")]
        public int PostId { get; set; }
        public string? ImageUrl { get; set; }
        public string? thumbnailUrl { get; set; }
        public bool IsPublished { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
        public virtual Post? Post { get; set; }
    }
}
