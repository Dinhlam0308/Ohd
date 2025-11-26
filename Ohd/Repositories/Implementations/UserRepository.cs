using Microsoft.EntityFrameworkCore;
using Ohd.Data;
using Ohd.Entities;
using Ohd.Repositories.Interfaces;
using Ohd.Utils;
using System.Linq;
using Ohd.DTOs.Common;

namespace Ohd.Repositories.Implementations
{
    public class UserRepository : IUserRepository
    {
        private readonly OhdDbContext _context;

        public UserRepository(OhdDbContext context)
        {
            _context = context;
        }

        public async Task<User?> GetByIdAsync(long id)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.Email == email);
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            return await _context.Users.AnyAsync(x => x.Email == email);
        }

        public async Task AddAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task SavePasswordHistoryAsync(long userId, byte[] oldHash)
        {
            var history = new password_history
            {
                user_id = userId,
                password_hash = oldHash,
                changed_at = DateTime.UtcNow
            };

            await _context.password_history.AddAsync(history);
            await _context.SaveChangesAsync();
        }

        // ============================
        // RESET PASSWORD TOKEN
        // ============================
        public async Task<password_reset_tokens?> GetResetTokenAsync(string token)
        {
            var hashed = TokenHasher.Hash(token);

            return await _context.password_reset_tokens
                .FirstOrDefaultAsync(x => x.token_hash.SequenceEqual(hashed));
        }

        public async Task CreateResetTokenAsync(long userId, string token, DateTime expiresAt)
        {
            var entity = new password_reset_tokens
            {
                user_id = userId,
                token_hash = TokenHasher.Hash(token),
                expires_at = expiresAt,
                created_at = DateTime.UtcNow
            };

            await _context.password_reset_tokens.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteResetTokenAsync(string token)
        {
            var hashed = TokenHasher.Hash(token);

            var entity = await _context.password_reset_tokens
                .FirstOrDefaultAsync(x => x.token_hash.SequenceEqual(hashed));

            if (entity != null)
            {
                _context.password_reset_tokens.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
        public async Task<string> GetUserRoleAsync(long userId)
        {
            var query =
                from ur in _context.user_roles
                join r in _context.roles on ur.role_id equals r.id
                where ur.user_id == userId
                select r.name;

            return await query.FirstOrDefaultAsync() ?? "User";
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _context.Users
                .OrderByDescending(u => u.Created_At)
                .ToListAsync();
        }

        public async Task<bool> DeleteAsync(long userId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == userId);
            if (user == null)
                return false;

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        
        }
        public async Task<int?> GetUserRoleIdAsync(long userId)
        {
            return await _context.user_roles
                .Where(ur => ur.user_id == userId)
                .Select(ur => (int?)ur.role_id)
                .FirstOrDefaultAsync();
        }
        public async Task<string?> GetUserRoleNameAsync(long userId)
        {
            var roleName = await (
                from ur in _context.user_roles
                join r in _context.roles on ur.role_id equals r.id
                where ur.user_id == userId
                select r.name
            ).FirstOrDefaultAsync();

            return roleName;    // ví dụ "Admin" / "DepartmentHead"
        }
        public async Task<IEnumerable<User>> GetUsersByRole(string roleName)
        {
            // Tìm role theo tên
            var role = await _context.roles
                .FirstOrDefaultAsync(r => r.name == roleName);

            if (role == null)
                return Enumerable.Empty<User>();

            // Join thủ công user_roles -> users
            var users = await _context.user_roles
                .Where(ur => ur.role_id == role.id)
                .Join(
                    _context.Users,
                    ur => ur.user_id,
                    u => u.Id,
                    (ur, u) => u
                )
                .ToListAsync();

            return users;
        }
        public async Task<PagedResult<User>> GetUsersAsync(string? search, int page, int pageSize)
        {
            var query = _context.Users.AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(u =>
                    u.Email.Contains(search) ||
                    u.Username.Contains(search)
                );
            }

            int total = await query.CountAsync();

            var items = await query
                .OrderByDescending(u => u.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<User>
            {
                Items = items,
                Total = total,
                Page = page,
                PageSize = pageSize
            };
        }

    }
}