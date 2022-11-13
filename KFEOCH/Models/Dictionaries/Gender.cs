namespace KFEOCH.Models.Dictionaries
{
    public class Gender
    {
        public int Id { get; set; }
        public string? NameArabic { get; set; }
        public string? NameEnglish { get; set; }
        public string? DescriptionArabic { get; set; }
        public string? DescriptionEnglish { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
