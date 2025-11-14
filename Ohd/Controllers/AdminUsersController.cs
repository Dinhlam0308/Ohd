using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ohd.DTOs.Auth;
using Ohd.Services;

namespace Ohd.Controllers
{
    [ApiController]
    [Route("api/admin/users")]
    [Authorize(Roles = "Admin")] // chỉ Admin mới gọi được
    public class AdminUsersController : ControllerBase
    {
        private readonly AuthService _auth;

        public AdminUsersController(AuthService auth)
        {
            _auth = auth;
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] AdminCreateUserRequest req)
        {
            var (ok, error, rawPassword) = await _auth.AdminCreateUser(req);
            if (!ok) return BadRequest(new { error });

            // Tuỳ bạn: có thể không trả password ra, chỉ gửi email
            return Ok(new
            {
                message = "User created and email queued",
                tempPassword = rawPassword  // trong thực tế có thể bỏ đi
            });
        }
    }
}