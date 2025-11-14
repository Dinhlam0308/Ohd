using Microsoft.EntityFrameworkCore;
using Ohd.Data;
using Ohd.DTOs.References;
using Ohd.Entities;

namespace Ohd.Services
{
    public class RequestStatusService
    {
        private readonly OhdDbContext _context;

        public RequestStatusService(OhdDbContext context)
        {
            _context = context;
        }

        public async Task<List<RequestStatus>> GetAllAsync()
        {
            return await _context.requeststatus
                .OrderBy(x => x.id)
                .ToListAsync();
        }

        public async Task<RequestStatus?> GetByIdAsync(int id)
        {
            return await _context.requeststatus.FindAsync(id);
        }

        public async Task<RequestStatus> CreateAsync(RequestStatusCreateDto dto)
        {
            var entity = new RequestStatus
            {
                code = dto.Code,
                name = dto.Name,
                is_final = dto.IsFinal,
                created_at = DateTime.UtcNow
            };

            _context.requeststatus.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> UpdateAsync(int id, RequestStatusUpdateDto dto)
        {
            var entity = await _context.requeststatus.FindAsync(id);
            if (entity == null) return false;

            entity.name = dto.Name;
            entity.is_final = dto.IsFinal;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _context.requeststatus.FindAsync(id);
            if (entity == null) return false;

            _context.requeststatus.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}