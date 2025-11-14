using Microsoft.EntityFrameworkCore;
using Ohd.Data;
using Ohd.DTOs.Facility;
using Ohd.Entities;

namespace Ohd.Services
{
    public class FacilityService
    {
        private readonly OhdDbContext _context;

        public FacilityService(OhdDbContext context)
        {
            _context = context;
        }

        public async Task<List<Facility>> GetAllAsync()
        {
            return await _context.Facilities
                .OrderBy(f => f.Name)
                .ToListAsync();
        }

        public async Task<Facility?> GetByIdAsync(int id)
        {
            return await _context.Facilities.FindAsync(id);
        }

        public async Task<Facility> CreateAsync(FacilityCreateDto dto)
        {
            var entity = new Facility
            {
                Name = dto.Name,
                Description = dto.Description,
                HeadUserId = dto.HeadUserId,
                CreatedAt = DateTime.UtcNow
            };

            _context.Facilities.Add(entity);
            await _context.SaveChangesAsync();

            return entity;
        }

        public async Task<bool> UpdateAsync(int id, FacilityUpdateDto dto)
        {
            var entity = await _context.Facilities.FindAsync(id);
            if (entity == null) return false;

            entity.Name = dto.Name;
            entity.Description = dto.Description;
            entity.HeadUserId = dto.HeadUserId;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _context.Facilities.FindAsync(id);
            if (entity == null) return false;

            _context.Facilities.Remove(entity);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}