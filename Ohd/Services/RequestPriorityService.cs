using Microsoft.EntityFrameworkCore;
using Ohd.Data;
using Ohd.DTOs.References;
using Ohd.Entities;

namespace Ohd.Services
{
    public class RequestPriorityService
    {
        private readonly OhdDbContext _context;

        public RequestPriorityService(OhdDbContext context)
        {
            _context = context;
        }

        public async Task<List<RequestPriority>> GetAllAsync()
        {
            return await _context.request_priorities
                .OrderBy(x => x.sort_order)
                .ToListAsync();
        }

        public async Task<RequestPriority?> GetByIdAsync(int id)
        {
            return await _context.request_priorities.FindAsync(id);
        }

        public async Task<RequestPriority> CreateAsync(RequestPriorityCreateDto dto)
        {
            var entity = new RequestPriority
            {
                code = dto.Code,
                name = dto.Name,
                sort_order = dto.SortOrder,
                created_at = DateTime.UtcNow
            };

            _context.request_priorities.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> UpdateAsync(int id, RequestPriorityUpdateDto dto)
        {
            var entity = await _context.request_priorities.FindAsync(id);
            if (entity == null) return false;

            entity.name = dto.Name;
            entity.sort_order = dto.SortOrder;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _context.request_priorities.FindAsync(id);
            if (entity == null) return false;

            _context.request_priorities.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }
    }

}