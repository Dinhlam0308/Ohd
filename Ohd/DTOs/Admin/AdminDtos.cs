namespace Ohd.DTOs.Admin
{
    // ------------ USER ------------
    public class AdminCreateUserRequest
    {
        public string Email { get; set; } = null!;
        public string? Username { get; set; }  // không bắt buộc
        public int? RoleId { get; set; }       // nếu muốn gán role luôn
    }

    public class AdminToggleUserDto
    {
        public bool IsActive { get; set; }
    }

}