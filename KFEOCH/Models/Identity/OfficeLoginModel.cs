namespace KFEOCH.Models.Identity
{
    public class OfficeLoginModel
    {
        public long LicenseId { get; set; }
        public int OfficeTypeId { get; set; }
        public string? Password { get; set; }
    }
}
