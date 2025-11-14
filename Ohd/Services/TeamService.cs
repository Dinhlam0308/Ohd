using Microsoft.EntityFrameworkCore;
using Ohd.Data;
using Ohd.DTOs.Teams;
using Ohd.Entities;

namespace Ohd.Services
{
    public class TeamService
    {
        private readonly OhdDbContext _context;

        public TeamService(OhdDbContext context)
        {
            _context = context;
        }

        public async Task<List<Teams>> GetAllAsync()
        {
            return await _context.teams
                .OrderBy(x => x.team_name)
                .ToListAsync();

        }
        

        public async Task<Teams?> GetByIdAsync(int id)
        {
            return await _context.teams.FindAsync(id);
        }

        public async Task<Teams> CreateAsync(TeamCreateDto dto)
        {
            var entity = new Teams
            {
                team_name = dto.TeamName,
                description = dto.Description,
                created_at = DateTime.UtcNow
            };

            _context.teams.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> UpdateAsync(int id, TeamUpdateDto dto)
        {
            var entity = await _context.teams.FindAsync(id);
            if (entity == null) return false;

            entity.team_name = dto.TeamName;
            entity.description = dto.Description;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _context.teams.FindAsync(id);
            if (entity == null) return false;

            _context.teams.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}