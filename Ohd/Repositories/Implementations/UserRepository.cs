using Microsoft.EntityFrameworkCore;
using Ohd.Data;
using Ohd.Entities;
using Ohd.Repositories.Interfaces;
using Ohd.Utils;
using System.Linq;

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
    }
}
