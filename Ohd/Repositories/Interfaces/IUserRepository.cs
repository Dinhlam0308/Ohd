using Ohd.Entities;

namespace Ohd.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetByIdAsync(long id);
        Task<User?> GetByEmailAsync(string email);
        Task<bool> EmailExistsAsync(string email);
        Task AddAsync(User user);
        Task SaveChangesAsync();

        Task SavePasswordHistoryAsync(long userId, byte[] oldHash);

        Task<password_reset_tokens?> GetResetTokenAsync(string token);
        Task CreateResetTokenAsync(long userId, string token, DateTime expiresAt);
        Task DeleteResetTokenAsync(string token);
    }
}