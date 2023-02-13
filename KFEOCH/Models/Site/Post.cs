using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KFEOCH.Models.Site
{
    public class Page
    {
        public Page()
        {

        }

        public int Id { get; set; }
        [Required]
        [MinLength(4)]
        public string? TitleArabic { get; set; }
        [Required]
        [MinLength(4)]
        public string? TitleEnglish { get; set; }
        public string? SubtitleArabic { get; set; }
        public string? SubtitleEnglish { get; set; }
        public string? BodyArabic { get; set; }
        public string? BodyEnglish { get; set; }
        public string? ImageUrl { get; set; }
        public string? ThumbnailUrl { get; set; }
        public string? Template { get; set; }
        public DateTime? CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime? PublishDate { get; set; } = DateTime.UtcNow;
        [Required]
        public string? HostUrl { get; set; }
        public bool IsPublished { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
        public virtual ICollection<Post>? Posts { get; set; }
    }

    public class PageBindingModel
    {
        public int Id { get; set; }
        public string? TitleArabic { get; set; }
        public string? TitleEnglish { get; set; }
        public string? SubtitleArabic { get; set; }
        public string? SubtitleEnglish { get; set; }
        public string? BodyArabic { get; set; }
        public string? BodyEnglish { get; set; }
        public DateTime? PublishDate { get; set; }
        public bool IsPublished { get; set; }
    }



    public class Post
    {
        public Post()
        {
        }
        public Post(PostBindingModel model)
        {
            Id = model.Id;
            TitleArabic = model.TitleArabic;
            TitleEnglish = model.TitleEnglish;
            SubtitleArabic = model.SubtitleArabic;
            SubtitleEnglish = model.SubtitleEnglish;
            BodyArabic = model.BodyArabic;
            BodyEnglish = model.BodyEnglish;
            PageId = model.PageId;
            CategoryId = model.CategoryId;
            IsPublished = model.IsPublished;
            PublishDate = model.PublishDate;
            Order = model.Order;
        }

        public Post(PostTitleBindingModel model)
        {
            TitleArabic = model.TitleEnglish;
            TitleEnglish = model.TitleEnglish;
            PageId = model.PageId;
        }
        public int Id { get; set; }
        [Required]
        [MinLength(4)]
        public string? TitleArabic { get; set; }
        [Required]
        [MinLength(4)]
        public string? TitleEnglish { get; set; }
        public string? SubtitleArabic { get; set; }
        public string? SubtitleEnglish { get; set; }
        public string? BodyArabic { get; set; }
        public string? BodyEnglish { get; set; }
        [ForeignKey("Page")]
        public int PageId { get; set; }
        [ForeignKey("Category")]
        public int? CategoryId { get; set; }
        public string? ImageUrl { get; set; }
        public string? ThumbnailUrl { get; set; }
        public DateTime? CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime? PublishDate { get; set; } = DateTime.UtcNow;
        public string? Url { get; set; }
        public bool IsPublished { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
        public int? Order { get; set; }
        public virtual Page? Page { get; set; }
        public virtual PostCategory? Category { get; set; }
        public virtual ICollection<Section>? Sections { get; set; }
    }


    
    public class PostBindingModel
    {
        public int Id { get; set; }
        public string? TitleArabic { get; set; }
        public string? TitleEnglish { get; set; }
        public string? SubtitleArabic { get; set; }
        public string? SubtitleEnglish { get; set; }
        public string? BodyArabic { get; set; }
        public string? BodyEnglish { get; set; }
        public int PageId { get; set; }
        public int? CategoryId { get; set; }
        public DateTime? PublishDate { get; set; }
        public int? Order { get; set; }
        public bool IsPublished { get; set; }
    }

    public class PostTitleBindingModel
    {
        public int PageId { get; set; }
        public string? TitleEnglish { get; set; }
    }


    public class Section
    {
        public Section()
        {
        }

        public Section(SectionBindingModel model)
        {
            Id = model.Id;
            TitleArabic = model.TitleArabic;
            TitleEnglish = model.TitleEnglish;
            SubtitleArabic = model.SubtitleArabic;
            SubtitleEnglish = model.SubtitleEnglish;
            BodyArabic = model.BodyArabic;
            BodyEnglish = model.BodyEnglish;
            PostId = model.PostId;
            IsPublished = model.IsPublished;
            PublishDate = model.PublishDate;
            Order = model.Order;
        }

        public Section(SectionWithImageBindingModel model)
        {
            TitleArabic = model.TitleArabic;
            TitleEnglish = model.TitleEnglish;
            PostId = model.PostId;
        }
        public int Id { get; set; }
        [Required]
        [MinLength(4)]
        public string? TitleArabic { get; set; }
        [Required]
        [MinLength(4)]
        public string? TitleEnglish { get; set; }
        public string? SubtitleArabic { get; set; }
        public string? SubtitleEnglish { get; set; }
        public string? BodyArabic { get; set; }
        public string? BodyEnglish { get; set; }
        [ForeignKey("Post")]
        public int PostId { get; set; }
        public string? ImageUrl { get; set; }
        public string? ThumbnailUrl { get; set; }
        public DateTime? CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime? PublishDate { get; set; } = DateTime.UtcNow;
        public string? Url { get; set; }
        public bool IsPublished { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
        public int? Order { get; set; }
        public virtual Post? Post { get; set; }
    }

    public class SectionBindingModel
    {
        public int Id { get; set; }
        public string? TitleArabic { get; set; }
        public string? TitleEnglish { get; set; }
        public string? SubtitleArabic { get; set; }
        public string? SubtitleEnglish { get; set; }
        public string? BodyArabic { get; set; }
        public string? BodyEnglish { get; set; }
        public int PostId { get; set; }
        public DateTime? PublishDate { get; set; }
        public bool IsPublished { get; set; }
        public int? Order { get; set; }
    }

    public class SectionWithImageBindingModel
    {
        public string? TitleArabic { get; set; }
        public string? TitleEnglish { get; set; }
        public int PostId { get; set; }
        public IFormFile? Image { get; set; }
    }

    public class PostCategory
    {
        public int Id { get; set; }
        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string? NameArabic { get; set; }
        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string? NameEnglish { get; set; }
        public string? DescriptionArabic { get; set; }
        public string? DescriptionEnglish { get; set; }
        public bool IsDeleted { get; set; } = false;
    }

    public class PostViewModel
    {
        public PostViewModel()
        {

        }

        public PostViewModel(Post model)
        {
            Id = model.Id;
            TitleArabic = model.TitleArabic;
            TitleEnglish = model.TitleEnglish;
            SubtitleArabic = model.SubtitleArabic;
            SubtitleEnglish = model.SubtitleEnglish;
            BodyArabic = model.BodyArabic;
            BodyEnglish = model.BodyEnglish;

            PageId = model.PageId;
            PageTitleArabic = model.Page.TitleArabic;
            PageTitleEnglish = model.Page.TitleEnglish;
            PageSubtitleArabic = model.Page.SubtitleArabic;
            PageSubtitleEnglish = model.Page.SubtitleEnglish;
            PageBodyArabic = model.Page.BodyArabic;
            PageBodyEnglish = model.Page.BodyEnglish;

            CategoryId = model.CategoryId;
            CategoryNameArabic = model.Category?.NameArabic;
            CategoryNameEnglish = model.Category?.NameEnglish;
            ImageUrl = model.ImageUrl;
            ThumbnailUrl = model.ThumbnailUrl;
            CreatedDate = model.CreatedDate;
            PublishDate = model.PublishDate;
            Url = model.Url;
            PageUrl = model.Page?.HostUrl;
            Order = model.Order;
        }

        public int Id { get; set; }
        public string? TitleArabic { get; set; }
        public string? TitleEnglish { get; set; }
        public string? SubtitleArabic { get; set; }
        public string? SubtitleEnglish { get; set; }
        public string? BodyArabic { get; set; }
        public string? BodyEnglish { get; set; }

        public int PageId { get; set; }
        public string? PageTitleArabic { get; set; }
        public string? PageTitleEnglish { get; set; }
        public string? PageSubtitleArabic { get; set; }
        public string? PageSubtitleEnglish { get; set; }
        public string? PageBodyArabic { get; set; }
        public string? PageBodyEnglish { get; set; }

        public int? CategoryId { get; set; }
        public string? CategoryNameArabic { get; set; }
        public string? CategoryNameEnglish { get; set; }
        public string? ImageUrl { get; set; }
        public string? ThumbnailUrl { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? PublishDate { get; set; }
        public string? Url { get; set; }
        public string PageUrl { get; set; }
        public int? Order { get; set; }
    }


    
}
