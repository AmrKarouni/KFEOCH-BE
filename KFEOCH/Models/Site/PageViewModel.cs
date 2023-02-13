namespace KFEOCH.Models.Site
{
    public class PageViewModel
    {
        public int Id { get; set; }
        public string? TitleArabic { get; set; }
        public string? TitleEnglish { get; set; }
        public string? SubtitleArabic { get; set; }
        public string? SubtitleEnglish { get; set; }
        public string? BodyArabic { get; set; }
        public string? BodyEnglish { get; set; }
        public string? ImageUrl { get; set; }
        public string? ThumbnailUrl { get; set; }
        public string? Template { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? PublishDate { get; set; }
        public string? HostUrl { get; set; }
        public bool IsPublished { get; set; }
        public bool IsDeleted { get; set; }
        public virtual IEnumerable<PostWithoutBodyViewModel>? Posts { get; set; }
    }

    public class PostWithoutBodyViewModel
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
        public string? CategoryNameArabic { get; set; }
        public string? CategoryNameEnglish { get; set; }
        public string? ImageUrl { get; set; }
        public string? ThumbnailUrl { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? PublishDate { get; set; }
        public string? Url { get; set; }
        public bool IsPublished { get; set; }
        public bool IsDeleted { get; set; }
        public int? Order { get; set; }
    }
}
