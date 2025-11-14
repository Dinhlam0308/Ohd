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
            var key = config["Jwt:Key"];
            if (string.IsNullOrWhiteSpace(key))
                throw new Exception("Jwt:Key is missing in configuration");
            _key = key;

            var issuer = config["Jwt:Issuer"];
            if (string.IsNullOrWhiteSpace(issuer))
                throw new Exception("Jwt:Issuer is missing in configuration");
            _issuer = issuer;

            var audience = config["Jwt:Audience"];
            if (string.IsNullOrWhiteSpace(audience))
                throw new Exception("Jwt:Audience is missing in configuration");
            _audience = audience;

            _expiresMinutes = int.TryParse(config["Jwt:ExpiresMinutes"], out var m) ? m : 60;

            _context = context;
        }

        public string GenerateToken(long userId, string email)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var keyBytes = Encoding.UTF8.GetBytes(_key);

            var roles = (
                from ur in _context.user_roles
                join r in _context.roles on ur.role_id equals r.id
                where ur.user_id == userId
                select r.name
            ).ToList();

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, email ),
                new Claim(ClaimTypes.NameIdentifier, userId.ToString())
            };

            foreach (var role in roles)
            {
                if (!string.IsNullOrWhiteSpace(role))
                    claims.Add(new Claim(ClaimTypes.Role, role));
            }

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

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
