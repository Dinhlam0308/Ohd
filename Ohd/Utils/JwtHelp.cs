using Microsoft.IdentityModel.Tokens;
using Ohd.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Ohd.Utils
{
    public class JwtHelper
    {
        private readonly string _key;
        private readonly string _issuer;
        private readonly string _audience;
        private readonly int _expiresMinutes;
        private readonly OhdDbContext _context;

        public JwtHelper(IConfiguration config, OhdDbContext context)
        {
            _key = config["Jwt:Key"] ?? throw new Exception("Jwt:Key missing");
            _issuer = config["Jwt:Issuer"] ?? throw new Exception("Jwt:Issuer missing");
            _audience = config["Jwt:Audience"] ?? throw new Exception("Jwt:Audience missing");
            _expiresMinutes = int.TryParse(config["Jwt:ExpiresMinutes"], out var m) ? m : 60;

            _context = context;
        }

        // ==========================================
        // 1️⃣ Generate token with ROLE (FE cần cái này)
        // ==========================================
        public string GenerateToken(long userId, string email, string role)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var keyBytes = Encoding.UTF8.GetBytes(_key);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, email),
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                new Claim(ClaimTypes.Role, role)
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(_expiresMinutes),
                Issuer = _issuer,
                Audience = _audience,
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(keyBytes),
                    SecurityAlgorithms.HmacSha256Signature
                )
            };

            return tokenHandler.WriteToken(tokenHandler.CreateToken(tokenDescriptor));
        }


        // ==========================================
        // 2️⃣ Generate token (auto lấy role từ DB)
        // ==========================================
        public string GenerateToken(long userId, string email)
        {
            var role = (
                from ur in _context.user_roles
                join r in _context.roles on ur.role_id equals r.id
                where ur.user_id == userId
                select r.name
            ).FirstOrDefault() ?? "User";

            return GenerateToken(userId, email, role);
        }

    }
}
