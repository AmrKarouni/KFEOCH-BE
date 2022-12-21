namespace KFEOCH.Models.Site
{
    public class PostBindingModel
    {
        public string? TitleArabic { get; set; }
        public string? TitleEnglish { get; set; }
        public string? SubTitleArabic { get; set; }
        public string? SubTitleEnglish { get; set; }
        public string? BodyArabic { get; set; }
        public string? BodyEnglish { get; set; }
        public string? Url { get; set; }
        public IFormFile? Image { get; set; }
        public int TypeId { get; set; }
        public bool IsPublished { get; set; }
    }
}
