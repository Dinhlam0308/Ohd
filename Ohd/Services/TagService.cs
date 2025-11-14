using Microsoft.EntityFrameworkCore;
using Ohd.Data;
using Ohd.DTOs.References;
using Ohd.Entities;

namespace Ohd.Services
{
    public class TagService
    {
        private readonly OhdDbContext _context;

        public TagService(OhdDbContext context)
        {
            _context = context;
        }

        public async Task<List<Tags>> GetAllAsync()
        {
            return await _context.tags
                .OrderBy(x => x.name)
                .ToListAsync();
        }

        public async Task<Tags?> GetByIdAsync(int id)
        {
            return await _context.tags.FindAsync(id);
        }

        public async Task<Tags> CreateAsync(TagCreateDto dto)
        {
            var entity = new Tags
            {
                code = dto.Code,
                name = dto.Name,
                created_at = DateTime.UtcNow
            };

            _context.tags.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> UpdateAsync(int id, TagUpdateDto dto)
        {
            var entity = await _context.tags.FindAsync(id);
            if (entity == null) return false;

            entity.name = dto.Name;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _context.tags.FindAsync(id);
            if (entity == null) return false;

            _context.tags.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}