using Microsoft.EntityFrameworkCore;
using Ohd.Data;
using Ohd.DTOs.Requests;
using Ohd.Entities;

namespace Ohd.Services
{
    public class NotificationService
    {
        private readonly OhdDbContext _context;

        public NotificationService(OhdDbContext context)
        {
            _context = context;
        }

        public async Task<List<Notification>> GetByUserAsync(long userId)
        {
            return await _context.notifications
                .Where(x => x.sent_to_user_id == userId)
                .OrderByDescending(x => x.created_at)
                .ToListAsync();
        }

        public async Task<Notification> CreateAsync(NotificationCreateDto dto)
        {
            var entity = new Notification
            {
                request_id = dto.RequestId,
                sent_to_user_id = dto.SentToUserId,
                message = dto.Message,
                created_at = DateTime.UtcNow,
                is_read = false
            };

            _context.notifications.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> MarkReadAsync(long id)
        {
            var entity = await _context.notifications.FindAsync(id);
            if (entity == null) return false;

            entity.is_read = true;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}