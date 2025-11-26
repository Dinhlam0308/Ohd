using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
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
        // LOGIN
        // ==========================
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var (ok, error, token, requireChangePassword, role) = await _auth.Login(request);

            if (!ok || token == null)
                return Unauthorized(new { message = error ?? "Email hoặc mật khẩu không đúng" });

            return Ok(new
            {
                token,
                role,
                requireChangePassword
            });

        }

        // ==========================
        // CHANGE PASSWORD (first login)
        // ==========================
        [HttpPost("change-password-first-login")]
        [Authorize]
        public async Task<IActionResult> ChangePasswordFirstLogin([FromBody] ChangePasswordFirstLoginRequest request)
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

        // ==========================
        // FORGOT PASSWORD → gửi email chứa link reset
        // ==========================
        [HttpPost("forgot-password")]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var (ok, error) = await _auth.ForgotPasswordAsync(request.Email);

            if (!ok)
                return BadRequest(new { message = error });

            return Ok(new
            {
                message = "Nếu email tồn tại trong hệ thống, chúng tôi đã gửi hướng dẫn đặt lại mật khẩu."
            });
        }

        // ==========================
        // RESET PASSWORD (từ link email)
        // ==========================
        [HttpPost("reset-password")]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (request.NewPassword != request.ConfirmNewPassword)
                return BadRequest(new { message = "Mật khẩu mới và xác nhận không trùng khớp" });

            var (ok, error) = await _auth.ResetPasswordAsync(request.Token, request.NewPassword);

            if (!ok)
                return BadRequest(new { message = error });

            return Ok(new { message = "Đặt lại mật khẩu thành công." });
        }
        [HttpPost("google-login")]
        [AllowAnonymous]
        public async Task<IActionResult> GoogleLogin([FromBody] GoogleLoginRequest request)
        {
            if (string.IsNullOrEmpty(request.Credential))
                return BadRequest(new { message = "Credential is required" });

            var (ok, error, token) = await _auth.GoogleLoginAsync(request.Credential, request.Email);

            if (!ok)
                return Unauthorized(new { message = error });

            return Ok(new { token });
        } 
    }

}
