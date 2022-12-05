namespace KFEOCH.Models.Identity
{
    public class OfficeChangePasswordModel
    {
        public string? UserName { get; set; }
        public string? CurrentPassword { get; set; }
        public string? NewPassword { get; set; }
    }
}
