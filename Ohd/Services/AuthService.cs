using Microsoft.EntityFrameworkCore;
using Ohd.Data;
using Ohd.DTOs.Auth;
using Ohd.Entities;
using Ohd.Utils;

namespace Ohd.Services
{
    public class AuthService
    {
        private readonly OhdDbContext _context;
        private readonly JwtHelper _jwt;

        public AuthService(OhdDbContext context, JwtHelper jwt)
        {
            _context = context;
            _jwt = jwt;
        }
        
        public async Task<string?> Login(LoginRequest request)
        {
            var user = await _context.Users
                .SingleOrDefaultAsync(x => x.Email == request.Email);

            if (user == null)
                return null;

            if (!user.Is_Active)
                return null;

            if (!PasswordHasher.Verify(request.Password, user.Password_Hash))
                return null;

            // sinh JWT từ Id + Email
            return _jwt.GenerateToken(user.Id, user.Email ?? string.Empty);
        }
        
        public async Task<(bool ok, string? error, string? rawPassword)> AdminCreateUser(AdminCreateUserRequest req)
        {
            // Check for duplicate email 
            var existed = await _context.Users.AnyAsync(u => u.Email == req.Email);
            if (existed)
                return (false, "Email already exists", null);

            // password random
            var rawPassword = PasswordGenerator.Generate(10);
            var hash = PasswordHasher.Hash(rawPassword);

            // create user
            var user = new User
            {
                Username = string.IsNullOrWhiteSpace(req.Username) ? req.Email : req.Username,
                Email = req.Email,
                Password_Hash = hash,
                Is_First_Login = true,
                Is_Active = true,
                Created_At = DateTime.UtcNow,
                Password_Last_Changed_At = null
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // 4. Gán role nếu có
            if (req.RoleId.HasValue)
            {
                var userRole = new user_roles
                {
                    user_id = user.Id,
                    role_id = req.RoleId.Value,
                    assigned_at = DateTime.UtcNow
                };
                _context.user_roles.Add(userRole);
            }
            
            var bodyHtml = $@"
                <p> {user.Username},</p>
                <p>The Online Help Desk system has created an account for you</p>
                <p><b>Email:</b> {user.Email}</p>
                <p><b>Temporary password:</b> {rawPassword}</p>
                <p>Please log in and change your password upon your frist login.</p>
            ";

            var msg = new OutboxMessage
            {
                recipient_email = user.Email!,
                subject = "Your Help Desk account ",
                body_html = bodyHtml,
                status = "Pending",    // map với cột ENUM('Pending','Sent','Failed')
                attempts = 0,
                last_attempt_at = null,
                created_at = DateTime.UtcNow
            };

            _context.outbox_messages.Add(msg);

            await _context.SaveChangesAsync();

            // Có thể không trả rawPassword nếu bạn muốn chỉ gửi qua email
            return (true, null, rawPassword);
        }

        // Change password after first login
        public async Task<(bool ok, string? error)> ChangePasswordFirstLogin(
            long userId,
            string oldPassword,
            string newPassword)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
                return (false, "User not found");

            if (!user.Is_First_Login)
                return (false, "First login already completed");

            if (!PasswordHasher.Verify(oldPassword, user.Password_Hash))
                return (false, "Old password incorrect");

            var newHash = PasswordHasher.Hash(newPassword);

            // Save password history to database password_history
            var history = new password_history
            {
                user_id = user.Id,
                password_hash = user.Password_Hash,
                changed_at = DateTime.UtcNow
            };
            _context.password_history.Add(history);

            // Update user
            user.Password_Hash = newHash;
            user.Is_First_Login = false;
            user.Password_Last_Changed_At = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return (true, null);
        }
    }
}
