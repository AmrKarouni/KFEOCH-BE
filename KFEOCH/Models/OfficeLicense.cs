using KFEOCH.Models.Binding;
using KFEOCH.Models.Dictionaries;
using System.ComponentModel.DataAnnotations.Schema;

namespace KFEOCH.Models
{
    public class OfficeLicense
    {
        public OfficeLicense()
        {

        }
        public int Id { get; set; }
        [ForeignKey("Office")]
        public int OfficeId { get; set; }

        [ForeignKey("PaymentType")]
        public int PaymentTypeId { get; set; }
        public DateTime CreateDate { get; set; } = DateTime.UtcNow;
        public DateTime? RegistrationStartDate { get; set; }
        public DateTime? RegistrationEndDate { get; set; }
        public string? DocumentUrl { get; set; }
        public double? PaymentAmount { get; set; }
        public string? PaymentNumber { get; set; }
        public bool? IsPaid { get; set; } = false;
        public bool? IsApproved { get; set; } = false;
        public bool? IsRejected { get; set; } = false;
        public bool? IsCanceled{ get; set; } = false;
        public virtual Office? Office { get; set; }
        public virtual PaymentType? PaymentType { get; set; }
    }
}
