namespace KFEOCH.Models
{
    public class OwnerFileModel
    {
        public IFormFile File { get; set; }
        public int OwnerId { get; set; }
        public int TypeId { get; set; }
    }
}
