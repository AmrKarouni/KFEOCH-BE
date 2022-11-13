namespace KFEOCH.Models.Identity
{
    public class OfficeChangePasswordModel
    {
        public long LicenseId { get; set; }
        public int OfficeTypeId { get; set; }
        public string? CurrentPassword { get; set; }
        public string? NewPassword { get; set; }
    }
}
