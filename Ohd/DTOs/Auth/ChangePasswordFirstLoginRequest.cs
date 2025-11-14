namespace Ohd.DTOs.Auth
{
    public class ChangePasswordFirstLoginRequest
    {
        public long UserId { get; set; }
        public string OldPassword { get; set; } = null!;
        public string NewPassword { get; set; } = null!;
        public string ConfirmNewPassword { get; set; } = null!; 
    }
}