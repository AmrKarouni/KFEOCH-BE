namespace KFEOCH.Models.Identity
{
    public class AdminChangePasswordModel
    {
        public string? UserName { get; set; }
        public string? CurrentPassword { get; set; }
        public string? NewPassword { get; set; }

    }
}
