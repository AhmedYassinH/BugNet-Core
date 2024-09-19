namespace BugNetCore.Services.DataServices.Settings
{
    public class JwtOptions
    {
        public string Issuer { get; set; } = string.Empty;
        public string Audience { get; set; } = string.Empty;
        public string SignKey { get; set; } = string.Empty;
        public int Lifetime { get; set; }
        public string EmailVerificationRoute { get; set; } = string.Empty;
        public string PasswordResetVerificationRoute { get; set; } = string.Empty;
    }
}
