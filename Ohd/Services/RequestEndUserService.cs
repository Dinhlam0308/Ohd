using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Ohd.Data;
using Ohd.Dtos.Requests;
using Ohd.Entities;

namespace Ohd.Services
{
    public class RequestEndUserService : IRequestEndUserService
    {
        private readonly OhdDbContext _db;
        private readonly ILogger<RequestEndUserService> _logger;

        public RequestEndUserService(OhdDbContext db, ILogger<RequestEndUserService> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task<long> CreateRequestAsync(long userId, string? userEmail, CreateRequestDto dto, CancellationToken ct = default)
        {
            var unassigned = await _db.request_statuses
                .FirstOrDefaultAsync(s => s.Code == "unassigned", ct)
                ?? throw new Exception("Missing request_status 'unassigned'.");

            var now = DateTime.UtcNow;

            var req = new Request
            {
                RequestorId = userId,
                FacilityId = dto.FacilityId,
                SeverityId = dto.SeverityId,
                Title = dto.Title,
                Description = dto.Description,
                RequesterEmail = userEmail,
                StatusId = unassigned.Id,
                CreatedAt = now,
                UpdatedAt = now,
                DueDate = dto.DueDate
            };

            _db.requests.Add(req);
            await _db.SaveChangesAsync(ct);

            return req.Id;
        }

        public async Task<IEnumerable<RequestListItemDto>> GetMyRequestsAsync(long userId, CancellationToken ct = default)
        {
            var now = DateTime.UtcNow;

            return await (
                from r in _db.requests.Include(x => x.Severity)
                join f in _db.Facilities on r.FacilityId equals f.Id
                join st in _db.request_statuses on r.StatusId equals st.Id
                where r.RequestorId == userId
                orderby r.CreatedAt descending
                select new RequestListItemDto
                {
                    Id = r.Id,
                    Title = r.Title,
                    FacilityName = f.Name,
                    SeverityName = r.Severity.name,
                    SeverityCode = r.Severity.code,
                    StatusName = st.Name,
                    StatusCode = st.Code,
                    StatusColor = st.Color,
                    CreatedAt = r.CreatedAt,
                    DueDate = r.DueDate,
                    IsOverdue = st.IsOverdue || (r.DueDate < now)
                }
            ).ToListAsync(ct);
        }

        public async Task<RequestDetailDto?> GetMyRequestDetailAsync(long userId, long requestId, CancellationToken ct = default)
        {
            var r = await _db.requests
                .Include(x => x.Severity)
                .FirstOrDefaultAsync(x => x.Id == requestId && x.RequestorId == userId, ct);

            if (r == null) return null;

            var facility = await _db.Facilities.FirstAsync(f => f.Id == r.FacilityId, ct);
            var status = await _db.request_statuses.FirstAsync(s => s.Id == r.StatusId, ct);

            var comments = await _db.request_comments
                .Where(c => c.RequestId == r.Id)
                .OrderBy(c => c.CreatedAt)
                .Select(c => new RequestCommentDto
                {
                    Id = c.Id,
                    AuthorUserId = c.AuthorUserId,
                    Body = c.Body,
                    AttachmentsCount = c.AttachmentsCount,
                    CreatedAt = c.CreatedAt
                })
                .ToListAsync(ct);

            return new RequestDetailDto
            {
                Id = r.Id,
                Title = r.Title,
                Description = r.Description,
                FacilityId = facility.Id,
                FacilityName = facility.Name,
                SeverityId = r.SeverityId,
                SeverityName = r.Severity.name,
                SeverityCode = r.Severity.code,
                StatusId = status.Id,
                StatusCode = status.Code,
                StatusName = status.Name,
                StatusColor = status.Color,
                CreatedAt = r.CreatedAt,
                UpdatedAt = r.UpdatedAt,
                DueDate = r.DueDate,
                Remarks = r.Remarks,
                IsOverdue = status.IsOverdue || (r.DueDate < DateTime.UtcNow),
                Comments = comments
            };
        }

        public async Task<bool> CloseMyRequestAsync(long userId, long requestId, string reason, CancellationToken ct = default)
        {
            var r = await _db.requests.FirstOrDefaultAsync(x => x.Id == requestId && x.RequestorId == userId, ct);
            if (r == null) return false;

            var closed = await _db.request_statuses.FirstAsync(s => s.Code == "closed", ct);

            var now = DateTime.UtcNow;

            r.StatusId = closed.Id;
            r.Remarks = reason;
            r.CompletedAt = now;
            r.UpdatedAt = now;

            _db.request_comments.Add(new Request_Comments
            {
                RequestId = r.Id,
                AuthorUserId = userId,
                Body = $"[User closed request] Reason: {reason}",
                CreatedAt = now
            });

            await _db.SaveChangesAsync(ct);
            return true;
        }

        public async Task<long?> AddCommentAsync(long userId, long requestId, string body, CancellationToken ct = default)
        {
            var exists = await _db.requests.AnyAsync(r => r.Id == requestId && r.RequestorId == userId, ct);
            if (!exists) return null;

            var c = new Request_Comments
            {
                RequestId = requestId,
                AuthorUserId = userId,
                Body = body,
                CreatedAt = DateTime.UtcNow
            };

            _db.request_comments.Add(c);
            await _db.SaveChangesAsync(ct);

            return c.Id;
        }
    }
}
