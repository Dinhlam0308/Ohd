using Microsoft.AspNetCore.Mvc;
using Ohd.DTOs.Auth;
using Ohd.Services;
using System.Threading.Tasks;

namespace Ohd.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _auth;

        public AuthController(AuthService auth)
        {
            _auth = auth;
        }

        // ==========================
        // POST: /api/auth/login
        // Đăng nhập bằng email + password
        // ==========================
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var token = await _auth.Login(request);

            if (token == null)
                return Unauthorized(new { message = "Email hoặc mật khẩu không đúng" });

            return Ok(new { token });
        }

        // ==========================
        // POST: /api/auth/change-password-first-login
        // Đổi mật khẩu lần đầu (sau khi admin tạo tài khoản)
        // ==========================
        [HttpPost("change-password-first-login")]
        public async Task<IActionResult> ChangePasswordFirstLogin(
            [FromBody] ChangePasswordFirstLoginRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var (ok, error) = await _auth.ChangePasswordFirstLogin(
                request.UserId,
                request.OldPassword,
                request.NewPassword
            );

            if (!ok)
                return BadRequest(new { message = error });

            return Ok(new { message = "Đổi mật khẩu thành công" });
        }
    }
}