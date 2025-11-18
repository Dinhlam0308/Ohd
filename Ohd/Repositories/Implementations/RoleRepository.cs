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

        public async Task AssignRoleToUser(long userId, int roleId)
        {
            var ur = new user_roles
            {
                user_id = userId,
                role_id = roleId,
                assigned_at = DateTime.UtcNow
            };

            await _context.user_roles.AddAsync(ur);
            await _context.SaveChangesAsync();
        }
    }
}