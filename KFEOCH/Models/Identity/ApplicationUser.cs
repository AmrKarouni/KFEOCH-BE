using KFEOCH.Models.Dictionaries;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace KFEOCH.Models.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public long LicenseId { get; set; }
        [ForeignKey("OfficeType")]
        public int OfficeTypeId { get; set; }
        [ForeignKey("Office")]
        public int? OfficeId { get; set; }
        public bool IsPasswordChanged { get; set; }
        public bool IsActive { get; set; }
        public virtual OfficeType? OfficeType { get; set; }
        public virtual Office? Office { get; set; }
        public virtual ICollection<RefreshToken>? RefreshTokens { get; set; }

    }
}
