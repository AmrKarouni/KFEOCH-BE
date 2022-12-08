using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KFEOCH.Models.Dictionaries
{
    public class RequestType
    {
        public int Id { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 5)]
        public string? NameArabic { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 5)]
        public string? NameEnglish { get; set; }
        public string? DescriptionArabic { get; set; }
        public string? DescriptionEnglish { get; set; }
        [ForeignKey("Parent")]
        public int? ParentId { get; set; }
        public double Amount { get; set; }
        public bool NeedAdminApproval { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public virtual OfficeType? Parent { get; set; }
    }
    public class RequestTypeViewModel : RequestType
    {
        public string? ParentNameArabic { get; set;}
        public string? ParentNameEnglish { get; set; }
        public RequestTypeViewModel(RequestType model)
        {
            Id = model.Id;
            NameArabic = model.NameArabic;
            NameEnglish = model.NameEnglish;
            DescriptionArabic = model.DescriptionArabic;
            DescriptionEnglish = model.DescriptionEnglish;
            NeedAdminApproval = model.NeedAdminApproval;
            Amount = model.Amount;
            IsActive = model.IsActive;
            IsDeleted = model.IsDeleted;
            ParentId = model.ParentId;
            ParentNameArabic = model.Parent?.NameArabic;
            ParentNameEnglish = model.Parent?.NameEnglish;

        }
    }
}
