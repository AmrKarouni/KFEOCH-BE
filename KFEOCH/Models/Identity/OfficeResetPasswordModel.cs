namespace KFEOCH.Models.Identity
{
    public class OfficeResetPasswordModel
    {
        public long LicenseId { get; set; }
        public int OfficeTypeId { get; set; }
        public string? NewPassword { get; set; }
    }

    public class UserResetPasswordModel
    {
        public string? Id { get; set; }
        public string? NewPassword { get; set; }
    }
}
