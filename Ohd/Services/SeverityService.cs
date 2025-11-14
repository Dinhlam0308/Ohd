using Microsoft.EntityFrameworkCore;
using Ohd.Data;
using Ohd.DTOs.References;
using Ohd.Entities;

namespace Ohd.Services
{
    public class SeverityService
    {
        private readonly OhdDbContext _context;

        public SeverityService(OhdDbContext context)
        {
            _context = context;
        }

        public async Task<List<Severity>> GetAllAsync()
        {
            return await _context.severities
                .OrderBy(x => x.sort_order)
                .ToListAsync();
        }

        public async Task<Severity?> GetByIdAsync(int id)
        {
            return await _context.severities.FindAsync(id);
        }

        public async Task<Severity> CreateAsync(SeverityCreateDto dto)
        {
            var entity = new Severity
            {
                code = dto.Code,
                name = dto.Name,
                sort_order = dto.SortOrder,
                created_at = DateTime.UtcNow
            };

            _context.severities.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> UpdateAsync(int id, SeverityUpdateDto dto)
        {
            var entity = await _context.severities.FindAsync(id);
            if (entity == null) return false;

            entity.name = dto.Name;
            entity.sort_order = dto.SortOrder;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _context.severities.FindAsync(id);
            if (entity == null) return false;

            _context.severities.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}