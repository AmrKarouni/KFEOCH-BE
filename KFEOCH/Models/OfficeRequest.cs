using KFEOCH.Models.Dictionaries;
using System.ComponentModel.DataAnnotations.Schema;

namespace KFEOCH.Models
{
    public class OfficeRequest
    {
        public int Id { get; set; }
        [ForeignKey("Office")]
        public int OfficeId { get; set; }
        [ForeignKey("RequestType")]
        public int RequestTypeId { get; set; }
        public double Amount { get; set; }
        public bool IsPaid { get; set; } = false;
        public DateTime? SubmitDate { get; set; }
        public DateTime? CancelDate { get; set; }
        public DateTime? ApproveDate { get; set; }
        public DateTime? RejectDate { get; set; }
        public DateTime? DoneDate { get; set; }
        public bool IsCanceled { get; set; } = false;
        public bool IsApproved { get; set; } = false;
        public bool IsRejected { get; set; } = false;
        public bool IsDone { get; set; } = false;
        public virtual Office? Office { get; set; }
        public virtual RequestType? RequestType { get; set; }
    }
}
