﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KFEOCH.Models.Dictionaries
{
    public class OfficeActivity
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
        [ForeignKey("OfficeType")]
        public int OfficeTypeId { get; set; }
        public bool IsDeleted { get; set; }
        public virtual OfficeType? OfficeType { get; set; }
    }
}
