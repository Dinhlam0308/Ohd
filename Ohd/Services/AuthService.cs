using Ohd.DTOs.Auth;
using Ohd.Entities;
using Ohd.Repositories.Interfaces;
using Ohd.Utils;
using Google.Apis.Auth;
namespace Ohd.Services
{
    public class AuthService
    {
        private readonly IUserRepository _users;
        private readonly IRoleRepository _roles;
        private readonly IOutboxRepository _outbox;
        private readonly JwtHelper _jwt;

        public AuthService(
            IUserRepository users,
            IRoleRepository roles,
            IOutboxRepository outbox,
            JwtHelper jwt)
        {
            _users = users;
            _roles = roles;
            _outbox = outbox;
            _jwt = jwt;
        }

        // ==========================
        // LOGIN (Email + Password)
        // Trả về: ok, error, token, requireChangePassword
        // ==========================
        public async Task<(bool ok, string? error, string? token, bool requireChangePassword)> Login(LoginRequest request)
        {
            var user = await _users.GetByEmailAsync(request.Email);
            if (user == null || !user.Is_Active)
                return (false, "Email hoặc mật khẩu không đúng", null, false);

            if (!PasswordHasher.Verify(request.Password, user.Password_Hash))
                return (false, "Email hoặc mật khẩu không đúng", null, false);

            // Check yêu cầu đổi mật khẩu:
            // 1) Lần đầu đăng nhập
            // 2) Hoặc đã quá 6 tháng từ lần đổi mật khẩu gần nhất
            bool requireChangePassword = false;

            if (user.Is_First_Login)
            {
                requireChangePassword = true;
            }
            else
            {
                var lastChanged = user.Password_Last_Changed_At;
                if (!lastChanged.HasValue || lastChanged.Value <= DateTime.UtcNow.AddMonths(-6))
                {
                    requireChangePassword = true;
                }
            }

            var token = _jwt.GenerateToken(user.Id, user.Email ?? string.Empty);
            return (true, null, token, requireChangePassword);
        }

        // ==========================
        // ADMIN CREATE USER
        // ==========================
        public async Task<(bool ok, string? error, string? rawPassword)> AdminCreateUser(AdminCreateUserRequest req)
        {
            if (await _users.EmailExistsAsync(req.Email))
                return (false, "Email already exists", null);

            var rawPassword = PasswordGenerator.Generate(10);
            var hash = PasswordHasher.Hash(rawPassword);

            var user = new User
            {
                Username = string.IsNullOrWhiteSpace(req.Username) ? req.Email : req.Username,
                Email = req.Email,
                Password_Hash = hash,
                Is_First_Login = true,              // bắt buộc đổi ở lần login đầu
                Is_Active = true,
                Created_At = DateTime.UtcNow,
                Password_Last_Changed_At = null     // null → coi như chưa đổi lần nào
            };

            await _users.AddAsync(user);

            if (req.RoleId.HasValue)
                await _roles.AssignRoleToUser(user.Id, req.RoleId.Value);

            var bodyHtml = $@"
                <p>Hello {user.Username},</p>
                <p>Your Online Help Desk account has been created.</p>
                <p><b>Email:</b> {user.Email}</p>
                <p><b>Temporary password:</b> {rawPassword}</p>
                <p>Please log in and change your password on first login.</p>
            ";

            await _outbox.QueueEmailAsync(user.Email!, "Your Help Desk Account", bodyHtml);

            return (true, null, rawPassword);
        }

        // ==========================
        // ĐỔI MẬT KHẨU (lần đầu hoặc sau 6 tháng)
        // FE gọi khi requireChangePassword = true
        // ==========================
        public async Task<(bool ok, string? error)> ChangePasswordFirstLogin(
            long userId,
            string oldPassword,
            string newPassword)
        {
            var user = await _users.GetByIdAsync(userId);
            if (user == null)
                return (false, "User not found");

            if (!PasswordHasher.Verify(oldPassword, user.Password_Hash))
                return (false, "Old password incorrect");

            // Lưu history mật khẩu cũ
            await _users.SavePasswordHistoryAsync(user.Id, user.Password_Hash);

            // Cập nhật mật khẩu mới
            user.Password_Hash = PasswordHasher.Hash(newPassword);
            user.Is_First_Login = false; // sau khi đổi xong thì bỏ cờ first login
            user.Password_Last_Changed_At = DateTime.UtcNow;

            await _users.SaveChangesAsync();
            return (true, null);
        }

        // ==========================
        // QUÊN MẬT KHẨU
        // Gửi email chứa mật khẩu tạm mới
        // ==========================
        public async Task<(bool ok, string? error)> ForgotPasswordAsync(string email)
        {
            var user = await _users.GetByEmailAsync(email);

            // Để tránh lộ email có tồn tại hay không, nếu không có user
            // thì vẫn trả ok = true, FE chỉ show message chung chung.
            if (user == null)
            {
                return (true, null);
            }

            // Lưu mật khẩu cũ vào history
            await _users.SavePasswordHistoryAsync(user.Id, user.Password_Hash);

            // Sinh mật khẩu tạm mới
            var rawPassword = PasswordGenerator.Generate(10);
            var newHash = PasswordHasher.Hash(rawPassword);

            user.Password_Hash = newHash;
            user.Is_First_Login = true;                 // reset xong bắt buộc đổi lại
            user.Password_Last_Changed_At = DateTime.UtcNow;

            var bodyHtml = $@"
                <p>Hello {user.Username},</p>
                <p>Your password has been reset.</p>
                <p><b>Temporary password:</b> {rawPassword}</p>
                <p>Please log in and change your password immediately.</p>
            ";

            await _outbox.QueueEmailAsync(user.Email!, "Password Reset for Online Help Desk", bodyHtml);

            await _users.SaveChangesAsync();
            return (true, null);
        
        }
        public async Task<(bool ok, string? error)> ResetPasswordAsync(string token, string newPassword)
        {
            // Tìm token trong DB
            var reset = await _users.GetResetTokenAsync(token);
            if (reset == null)
                return (false, "Token không hợp lệ hoặc đã hết hạn");

            if (reset.expires_at < DateTime.UtcNow)
                return (false, "Token đã hết hạn");

            // Lấy user
            var user = await _users.GetByIdAsync(reset.user_id);
            if (user == null)
                return (false, "User không tồn tại");

            // Lưu mật khẩu cũ vào history
            await _users.SavePasswordHistoryAsync(user.Id, user.Password_Hash);

            // Cập nhật mật khẩu mới
            user.Password_Hash = PasswordHasher.Hash(newPassword);
            user.Is_First_Login = false;
            user.Password_Last_Changed_At = DateTime.UtcNow;

            // Xóa token sau khi dùng
            await _users.DeleteResetTokenAsync(token);

            await _users.SaveChangesAsync();
            return (true, null);
        }
        public async Task<(bool ok, string? error, string? token)> GoogleLoginAsync(string credential, string email)
        {
            try
            {
                // 1) Xác minh Google ID Token
                var payload = await GoogleJsonWebSignature.ValidateAsync(credential);
                if (payload == null)
                    return (false, "Token Google không hợp lệ", null);

                var googleEmail = payload.Email?.ToLower();
                if (googleEmail != email.ToLower())
                    return (false, "Email không trùng khớp", null);

                // 2) Kiểm tra email trong hệ thống
                var user = await _users.GetByEmailAsync(googleEmail);
                if (user == null)
                    return (false, "Email không có quyền truy cập hệ thống", null);

                if (!user.Is_Active)
                    return (false, "Tài khoản bị vô hiệu hoá", null);

                // 3) Tạo JWT token
                var token = _jwt.GenerateToken(user.Id, user.Email!);

                return (true, null, token);
            }
            catch (Exception ex)
            {
                return (false, ex.Message, null);
            }
        }

    }
}
