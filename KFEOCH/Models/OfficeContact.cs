using KFEOCH.Models.Binding;
using KFEOCH.Models.Dictionaries;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KFEOCH.Models
{
    public class OfficeContact
    {
        public OfficeContact()
        {

        }
        public OfficeContact(OfficeContactBindingModel model)
        {
            OfficeId = model.OfficeId;
            ContactId = model.ContactId;
            PhoneNumber = model.PhoneNumber;
            AddedDate = DateTime.UtcNow;
            IsApproved = true;
            IsDeleted = false;
        }

        public int Id { get; set; }
        [ForeignKey("Office")]
        public int? OfficeId { get; set; }
        [ForeignKey("Contact")]
        public int? ContactId { get; set; }
        [DataType(DataType.PhoneNumber)]
        //[RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Not a valid phone number")]
        public string? PhoneNumber { get; set; }
        public DateTime? AddedDate { get; set; }
        public bool IsApproved { get; set; }
        public bool IsDeleted { get; set; } = false;
        public virtual Office? Office { get; set; }
        public virtual ContactType? Contact { get; set; }
    }
}
