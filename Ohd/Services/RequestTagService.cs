using Microsoft.EntityFrameworkCore;
using Ohd.Data;
using Ohd.DTOs.Requests;
using Ohd.Entities;

namespace Ohd.Services
{
    public class RequestTagService
    {
        private readonly OhdDbContext _context;

        public RequestTagService(OhdDbContext context)
        {
            _context = context;
        }

        public async Task<List<RequestTag>> GetByRequestAsync(long requestId)
        {
            return await _context.request_tags
                .Where(x => x.request_id == requestId)
                .ToListAsync();
        }

        public async Task<bool> AddTagAsync(RequestTagAssignDto dto)
        {
            if (await _context.request_tags
                    .AnyAsync(x => x.request_id == dto.RequestId && x.tag_id == dto.TagId))
                return true;

            var entity = new RequestTag
            {
                request_id = dto.RequestId,
                tag_id = dto.TagId,
                created_at = DateTime.UtcNow
            };

            _context.request_tags.Add(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveTagAsync(RequestTagAssignDto dto)
        {
            var entity = await _context.request_tags.FindAsync(dto.RequestId, dto.TagId);
            if (entity == null) return false;

            _context.request_tags.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}