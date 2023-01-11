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
        public int PageId { get; set; }
        public bool IsPublished { get; set; }
    }

    public class PostFileModel
    {
        public PostFileModel()
        {

        }
        public PostFileModel(PostBindingModel model)
        {
            Image = model.Image;
            PageId = model.PageId;
            PostId = PostId;
        }
        public IFormFile? Image { get; set; }
        public int PageId { get; set; }
        public int PostId { get; set; }
    }
}
