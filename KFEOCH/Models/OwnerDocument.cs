using KFEOCH.Models.Binding;
using KFEOCH.Models.Dictionaries;
using System.ComponentModel.DataAnnotations.Schema;

namespace KFEOCH.Models
{
    public class OwnerDocument
    {
        public OwnerDocument()
        {
        }
        public OwnerDocument(OwnerFileModel model)
        {
            OwnerId = model.OwnerId;
            TypeId = model.TypeId;
            AddedDate = DateTime.UtcNow;
            IsActive = true;
            IsApproved = true;
            IsDeleted = false;
        }

        public int Id { get; set; }
        [ForeignKey("Owner")]
        public int? OwnerId { get; set; }
        [ForeignKey("Type")]
        public int? TypeId { get; set; }
        public string? DocumentUrl { get; set; }
        public DateTime? AddedDate { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsApproved { get; set; }
        public bool? IsDeleted { get; set; }
        public virtual OfficeOwner? Owner { get; set; }
        public virtual OwnerDocumentType? Type { get; set; }
    }
}
