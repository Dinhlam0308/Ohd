using Microsoft.EntityFrameworkCore;
using Ohd.Data;
using Ohd.DTOs.Requests;
using Ohd.Entities;

namespace Ohd.Services
{
    public class RequestCommentService
    {
        private readonly OhdDbContext _context;

        public RequestCommentService(OhdDbContext context)
        {
            _context = context;
        }

        public async Task<List<Request_Comments>> GetByRequestAsync(long requestId)
        {
            return await _context.request_comments
                .Where(x => x.RequestId == requestId)
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();
        }
        
        public async Task<Request_Comments> CreateAsync(RequestCommentCreateDto dto)
        {
            var entity = new Request_Comments
            {
                RequestId = dto.RequestId,
                AuthorUserId = dto.AuthorUserId,
                Body = dto.Body,
                CreatedAt = DateTime.UtcNow,
                AttachmentsCount = 0
            };

            _context.request_comments.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> UpdateAsync(long id, RequestCommentUpdateDto dto)
        {
            var entity = await _context.request_comments.FindAsync(id);
            if (entity == null) return false;

            entity.Body = dto.Body;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(long id)
        {
            var entity = await _context.request_comments.FindAsync(id);
            if (entity == null) return false;

            _context.request_comments.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}