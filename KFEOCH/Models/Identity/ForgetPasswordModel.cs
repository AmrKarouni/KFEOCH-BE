using System.ComponentModel.DataAnnotations;

namespace KFEOCH.Models.Identity
{
    public class ForgetPasswordModel
    {
        [Required]
        [EmailAddress]
        public string? Email { get; set; }
        [Required]
        public string? ClientUri { get; set; }
    }
}
