using Microsoft.EntityFrameworkCore;
using Ohd.Data;
using Ohd.DTOs.Teams;
using Ohd.Entities;

namespace Ohd.Services
{
    public class UserTeamService
    {
        private readonly OhdDbContext _context;

        public UserTeamService(OhdDbContext context)
        {
            _context = context;
        }

        public async Task<List<UserTeam>> GetByUserAsync(long userId)
        {
            return await _context.user_teams
                .Where(x => x.user_id == userId)
                .ToListAsync();
        }

        public async Task<bool> AddUserToTeamAsync(UserTeamAssignDto dto)
        {
            if (await _context.user_teams
                    .AnyAsync(x => x.user_id == dto.UserId && x.team_id == dto.TeamId))
            {
                return true; // đã tồn tại
            }

            var entity = new UserTeam
            {
                user_id = dto.UserId,
                team_id = dto.TeamId,
                joined_at = DateTime.UtcNow
            };

            _context.user_teams.Add(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveUserFromTeamAsync(UserTeamAssignDto dto)
        {
            var entity = await _context.user_teams
                .FindAsync(dto.UserId, dto.TeamId);

            if (entity == null) return false;

            _context.user_teams.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}