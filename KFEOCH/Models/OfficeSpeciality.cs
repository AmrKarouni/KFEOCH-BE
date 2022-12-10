using KFEOCH.Models.Dictionaries;
using System.ComponentModel.DataAnnotations.Schema;

namespace KFEOCH.Models
{
    public class OfficeSpeciality
    {
        public OfficeSpeciality()
        {

        }
        public OfficeSpeciality(int officeId, int specialityId)
        {
            OfficeId = officeId;
            SpecialityId = specialityId;
            AddedDate = DateTime.UtcNow;
            IsApproved = true;
            IsDeleted = false;
        }

        public int Id { get; set; }
        [ForeignKey("Office")]
        public int? OfficeId { get; set; }
        [ForeignKey("Speciality")]
        public int? SpecialityId{ get; set; }
        public DateTime? AddedDate { get; set; }
        public bool IsApproved { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
        public virtual Office? Office { get; set; }
        public virtual Speciality? Speciality{ get; set; }
    }
}
