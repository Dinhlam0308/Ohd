namespace Ohd.DTOs.Auth
{
    public class AdminCreateUserRequest
    {
        public string Email { get; set; } = null!;
        public string? Username { get; set; }  // không bắt buộc, có thể dùng email thay thế
        public int? RoleId { get; set; }       // nếu muốn gán role luôn
    }
}