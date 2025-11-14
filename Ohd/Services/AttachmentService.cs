using Microsoft.EntityFrameworkCore;
using Ohd.Data;
using Ohd.DTOs.Requests;
using Ohd.Entities;

namespace Ohd.Services
{
    public class AttachmentService
    {
        private readonly OhdDbContext _context;

        public AttachmentService(OhdDbContext context)
        {
            _context = context;
        }

        public async Task<List<Attachment>> GetByRequestAsync(long requestId)
        {
            return await _context.attachments
                .Where(x => x.RequestId == requestId)
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();
        }

        public async Task<Attachment> CreateAsync(AttachmentCreateDto dto)
        {
            var entity = new Attachment
            {
                RequestId = dto.RequestId,
                UploadedByUserId = dto.UploadedByUserId,
                FileName = dto.FileName,
                MimeType = dto.MimeType,
                FileSizeBytes = dto.FileSizeBytes,
                StorageUrl = dto.StorageUrl,
                CreatedAt = DateTime.UtcNow
            };

            _context.attachments.Add(entity);
            await _context.SaveChangesAsync();

            return entity;
        }

        public async Task<bool> DeleteAsync(long id)
        {
            var entity = await _context.attachments.FindAsync(id);
            if (entity == null) return false;

            _context.attachments.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}