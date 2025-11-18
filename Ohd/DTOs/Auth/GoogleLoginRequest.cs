namespace Ohd.DTOs.Auth
{
    public class GoogleLoginRequest
    {
        public string Credential { get; set; } = string.Empty;  // Google ID Token
        public string Email { get; set; } = string.Empty;        // Extracted from FE
    }
}