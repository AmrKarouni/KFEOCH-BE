﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KFEOCH.Models.Dictionaries
{
    public class Governorate
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
        [ForeignKey("Country")]
        public int CountryId { get; set; }
        public bool IsDeleted { get; set; }
        public virtual Country? Country { get; set; }
        public virtual ICollection<Area> Areas { get; set; } = new List<Area>();
    }
}
