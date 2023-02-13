using System.ComponentModel.DataAnnotations;

namespace KFEOCH.Models
{
    public class SiteMessage
    {
        public SiteMessage()
        {

        }

        public SiteMessage(SiteMessageBindingModel model)
        {
            Name = model.Name;
            Email = model.Email;
            PhoneNumber = model.PhoneNumber;
            Subject = model.Subject;
        }
        public int Id { get; set; }
        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string? Name { get; set; }
        [Required]
        [EmailAddress]
        public string? Email { get; set; }
        [Required]
        [DataType(DataType.PhoneNumber)]
        public string? PhoneNumber { get; set; }
        [Required]
        public string? Subject { get; set; }
        public DateTime? CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime? ReadDate { get; set; }
        public bool IsRead { get; set; } = false;

    }

    public class SiteMessageBindingModel
    {
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Subject { get; set; }  
    }
}
