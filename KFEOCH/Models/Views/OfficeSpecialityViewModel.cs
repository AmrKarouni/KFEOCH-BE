namespace KFEOCH.Models.Views
{
    public class OfficeSpecialityViewModel
    {
        public OfficeSpecialityViewModel(OfficeSpeciality model)
        {
            Id = model.Id;
            OfficeId = model.OfficeId;
            SpecialityId = model.SpecialityId;
            NameArabic = model.Speciality?.NameArabic;
            NameEnglish = model.Speciality?.NameEnglish;
            AddedDate = model.AddedDate;
            IsApproved = model.IsApproved;
            IsDeleted = model.IsDeleted;
        }
        public int Id { get; set; }
        public int? OfficeId { get; set; }
        public int? SpecialityId { get; set; }
        public string? NameArabic { get; set; }
        public string? NameEnglish { get; set; }
        public DateTime? AddedDate { get; set; } 
        public bool IsApproved {get; set; }
        public bool IsDeleted {get; set; }

}
}
