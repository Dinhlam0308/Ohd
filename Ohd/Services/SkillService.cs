using Microsoft.EntityFrameworkCore;
using Ohd.Data;
using Ohd.DTOs.References;
using Ohd.Entities;

namespace Ohd.Services
{
    public class SkillService
    {
        private readonly OhdDbContext _context;

        public SkillService(OhdDbContext context)
        {
            _context = context;
        }

        public async Task<List<Skills>> GetAllAsync()
        {
            return await _context.skills
                .OrderBy(x => x.name)
                .ToListAsync();
        }

        public async Task<Skills?> GetByIdAsync(int id)
        {
            return await _context.skills.FindAsync(id);
        }

        public async Task<Skills> CreateAsync(SkillCreateDto dto)
        {
            var entity = new Skills
            {
                name = dto.Name,
                created_at = DateTime.UtcNow
            };

            _context.skills.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> UpdateAsync(int id, SkillUpdateDto dto)
        {
            var entity = await _context.skills.FindAsync(id);
            if (entity == null) return false;

            entity.name = dto.Name;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _context.skills.FindAsync(id);
            if (entity == null) return false;

            _context.skills.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}