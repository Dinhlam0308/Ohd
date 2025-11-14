using Microsoft.EntityFrameworkCore;
using Ohd.Data;
using Ohd.DTOs.Requests;
using Ohd.Entities;

namespace Ohd.Services
{
    public class RequestService
    {
        private readonly OhdDbContext _context;

        public RequestService(OhdDbContext context)
        {
            _context = context;
        }

        // Lấy tất cả request (chưa include facility)
        public async Task<List<Request>> GetAllAsync()
        {
            return await _context.requests
                .ToListAsync();
        }

        // Lấy 1 request theo Id
        public async Task<Request?> GetByIdAsync(long id)
        {
            return await _context.requests
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<Request> CreateAsync(RequestCreateDto dto)
        {
            var now = DateTime.UtcNow;

            var entity = new Request
            {
                RequestorId = dto.RequestorId,
                FacilityId = dto.FacilityId,
                Title = dto.Title,
                Description = dto.Description,
                SeverityId = dto.SeverityId,
                PriorityId = dto.PriorityId,
                StatusId = 1, // ví dụ: 1 = New / Open
                CreatedAt = now,
                UpdatedAt = now
            };

            _context.requests.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> UpdateAsync(long id, RequestUpdateDto dto)
        {
            var entity = await _context.requests.FindAsync(id);
            if (entity == null) return false;

            entity.Title = dto.Title;
            entity.Description = dto.Description;
            entity.PriorityId = dto.PriorityId;
            entity.Remarks = dto.Remarks;
            entity.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ChangeStatusAsync(long id, RequestChangeStatusDto dto)
        {
            var entity = await _context.requests.FindAsync(id);
            if (entity == null) return false;

            var fromStatus = entity.StatusId;
            entity.StatusId = dto.ToStatusId;
            entity.UpdatedAt = DateTime.UtcNow;

            var history = new request_history
            {
                request_id = id,
                from_status_id = fromStatus,
                to_status_id = dto.ToStatusId,
                changed_by_user_id = dto.ChangedByUserId,
                remarks = dto.Remarks,
                changed_at = DateTime.UtcNow
            };

            _context.request_history.Add(history);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(long id)
        {
            var entity = await _context.requests.FindAsync(id);
            if (entity == null) return false;

            _context.requests.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}