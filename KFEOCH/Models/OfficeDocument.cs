using KFEOCH.Models.Dictionaries;
using System.ComponentModel.DataAnnotations.Schema;

namespace KFEOCH.Models
{
    public class OfficeDocument
    {
        public OfficeDocument()
        {

        }

        public OfficeDocument(OfficeFileModel model)
        {
            OfficeId = model.OfficeId;
            TypeId = model.TypeId;
            AddedDate = DateTime.UtcNow;
            IsActive = true;
            IsApproved = true;
            IsDeleted = false;
        }
        public int Id { get; set; }
        [ForeignKey("Office")]
        public int? OfficeId { get; set; }
        [ForeignKey("Type")]
        public int? TypeId { get; set; }
        public string? DocumentUrl { get; set; }
        public DateTime? AddedDate { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsApproved { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
        public virtual Office? Office { get; set; }
        public virtual OfficeDocumentType? Type { get; set; }
    }
}
