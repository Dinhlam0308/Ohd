using Microsoft.EntityFrameworkCore;
using Ohd.Data;
using Ohd.DTOs.References;
using Ohd.Entities;

namespace Ohd.Services
{
    public class CategoryService
    {
        private readonly OhdDbContext _context;

        public CategoryService(OhdDbContext context)
        {
            _context = context;
        }

        public async Task<List<Category>> GetAllAsync()
        {
            return await _context.categories
                .OrderBy(x => x.name)
                .ToListAsync();
        }

        public async Task<Category?> GetByIdAsync(int id)
        {
            return await _context.categories.FindAsync(id);
        }

        public async Task<Category> CreateAsync(CategoryCreateDto dto)
        {
            var entity = new Category
            {
                name = dto.Name,
                parent_id = dto.ParentId,
                created_at = DateTime.UtcNow
            };

            _context.categories.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> UpdateAsync(int id, CategoryUpdateDto dto)
        {
            var entity = await _context.categories.FindAsync(id);
            if (entity == null) return false;

            entity.name = dto.Name;
            entity.parent_id = dto.ParentId;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _context.categories.FindAsync(id);
            if (entity == null) return false;

            _context.categories.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}