using Microsoft.EntityFrameworkCore;
using Ohd.Data;
using Ohd.Entities;
using Ohd.Repositories.Interfaces;

namespace Ohd.Repositories.Implementations
{
    public class RoleRepository : IRoleRepository
    {
        private readonly OhdDbContext _context;

        public RoleRepository(OhdDbContext context)
        {
            _context = context;
        }

        // 🟩 Gán role cho user
        public async Task AssignRoleToUser(long userId, int roleId)
        {
            var ur = new UserRoles
            {
                user_id = userId,
                role_id = roleId,
                assigned_at = DateTime.UtcNow
            };

            await _context.user_roles.AddAsync(ur);
            await _context.SaveChangesAsync();
        }

        // 🟥 Xoá tất cả role cũ của user
        public async Task RemoveAllRolesFromUser(long userId)
        {
            var roles = await _context.user_roles
                .Where(x => x.user_id == userId)
                .ToListAsync();

            if (roles.Any())
            {
                _context.user_roles.RemoveRange(roles);
                await _context.SaveChangesAsync();
            }
        }
    }
}