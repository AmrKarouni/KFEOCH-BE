namespace KFEOCH.Models
{

    public class FileModel
    {
        public IFormFile File { get; set; }
        public string? FileName { get; set; }
    }
    public class FilePathModel
    {
        public string? Path { get; set; }
        public string? ContentType { get; set; }
    }

    public class FileBytesModel
    {
        public byte[]? Bytes { get; set; } = null;
        public string? FileName { get; set; }
        public string? ContentType { get; set; }
    }
    public class ContentType
    {
        public string? Extension { get; set; }
        public string? cType { get; set; }
    }
}
