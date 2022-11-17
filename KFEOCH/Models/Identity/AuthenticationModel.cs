namespace KFEOCH.Models.Identity
{
    public class AuthenticationModel
    {
        public string? Message { get; set; }
        public bool IsAuthenticated { get; set; }
        public string? UserName { get; set; }
        public string? NameArabic { get; set; }
        public string? NameEnglish { get; set; }
        public string? Email { get; set; }
        public List<string>? Roles { get; set; }
        public string? Token { get; set; }
        public double TokenDurationM { get; set; }
        public DateTime? TokenExpiry { get; set; }
        public string? RefreshToken { get; set; }
        public double RefreshTokenDurationM { get; set;}
        public DateTime RefreshTokenExpiry { get; set; }
    }
}
