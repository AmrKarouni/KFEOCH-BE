namespace KFEOCH.Models
{
    public class PostFileModel
    {
        public IFormFile File { get; set; }
        public int TypeId { get; set; }
        public int PostId { get; set; }
    }
}
