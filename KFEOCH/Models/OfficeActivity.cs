using KFEOCH.Models.Dictionaries;
using System.ComponentModel.DataAnnotations.Schema;

namespace KFEOCH.Models
{
    public class OfficeActivity
    {
        public OfficeActivity()
        {

        }
        public OfficeActivity(int officeId, int activityId)
        {
            OfficeId = officeId;
            ActivityId = activityId;
            AddedDate = DateTime.UtcNow;
            IsApproved = true;
            IsDeleted = false;
        }
        public int Id { get; set; }
        [ForeignKey("Office")]
        public int? OfficeId { get; set; }
        [ForeignKey("Activity")]
        public int? ActivityId { get; set; }
        public DateTime? AddedDate { get; set; }
        public bool IsApproved { get; set; }
        public bool IsDeleted { get; set; }
        public virtual Office? Office { get; set; }
        public virtual Activity? Activity { get; set; }
    }
}
