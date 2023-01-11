using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        public virtual ICollection<Page>? Pages { get; set; }
    }
    public class Page
    {
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
        public string? ThumbnailUrl { get; set; }
        public DateTime? CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime? PublishDate { get; set; } = DateTime.UtcNow;
        public string? Url { get; set; }
        public bool IsPublished { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
        public virtual PostType? Type { get; set; }
        public virtual ICollection<Post>? Posts { get; set; }
    }

    public class Post
    {
        public Post()
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
            PageId = model.PageId;
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
        [ForeignKey("Page")]
        public int PageId { get; set; }
        public string? ImageUrl { get; set; }
        public string? ThumbnailUrl { get; set; }
        public DateTime? CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime? PublishDate { get; set; } = DateTime.UtcNow;
        public string? Url { get; set; }
        public bool IsPublished { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
        public virtual Page? Page { get; set; }
        public virtual ICollection<Post>? Sections { get; set; }
    }
}
