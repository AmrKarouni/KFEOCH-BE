﻿using System.ComponentModel.DataAnnotations;

namespace KFEOCH.Models.Dictionaries
{
    public class OwnerDocumentType
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
        public bool? HasForm { get; set; } = false;
        public string? FormUrl { get; set; }
        public bool IsDeleted { get; set; } = false;
    }
}
