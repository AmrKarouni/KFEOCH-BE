namespace KFEOCH.Models.Site
{
    public class Article
    {
        public int Id { get; set; }
        public string? HeadlineArabic { get; set; }
        public string? HeadlineEnglish { get; set; }
        public string? BodyArabic { get; set; }
        public string? BodyEnglish { get; set; }
        public string? ImageUrl { get; set; }
        public DateTime? NewsDate { get; set; }
        public DateTime? PublishDate { get; set; }
        public bool? ShowInHome { get; set; }
        public bool IsDeleted { get; set; } = false;
    }
}
