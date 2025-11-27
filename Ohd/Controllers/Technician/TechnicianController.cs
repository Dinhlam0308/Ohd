using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Ohd.Data;
using Ohd.DTOs.Technician;
using Ohd.Entities;
using System.Security.Claims;

namespace Ohd.Controllers
{
    [ApiController]
    [Route("api/technician")]
    [Authorize(Roles = "Technician")]
    public class TechnicianController : ControllerBase
    {
        private readonly OhdDbContext _db;
        private readonly Cloudinary _cloudinary;

        public TechnicianController(OhdDbContext db, Cloudinary cloudinary)
        {
            _db = db;
            _cloudinary = cloudinary;
        }

        private long GetCurrentUserId()
        {
            var claim = User.FindFirst("id") ?? User.FindFirst(ClaimTypes.NameIdentifier);
            return long.Parse(claim!.Value);
        }

        private DateTime? ConvertDueDate(TimeSpan? due)
        {
            if (due == null) return null;
            return DateTime.Today.Add(due.Value);
        }

        // =========================================================
        // 1) LẤY REQUEST CHO TECHNICIAN
        // =========================================================
       [HttpGet("requests")]
public async Task<IActionResult> GetMyRequests(
    int? statusId = null,
    int? severityId = null,
    long? facilityId = null,
    string? sort = "priority",
    int page = 1,
    int pageSize = 10)
{
    if (page < 1) page = 1;
    long techId = GetCurrentUserId();
    var now = DateTime.UtcNow;

    var query = _db.requests.AsNoTracking()
        .Where(r => r.AssigneeId == techId);

    if (statusId.HasValue) query = query.Where(r => r.StatusId == statusId);
    if (severityId.HasValue) query = query.Where(r => r.SeverityId == severityId);
    if (facilityId.HasValue) query = query.Where(r => r.FacilityId == facilityId);

    switch (sort?.ToLower())
    {
        case "severity":
            query = query.OrderByDescending(r => r.SeverityId);
            break;
        case "duedate":
            query = query.OrderBy(r => r.DueDate);
            break;
        case "createdatdesc":
            query = query.OrderByDescending(r => r.CreatedAt);
            break;
        default:
            query = query.OrderByDescending(r => r.SeverityId)
                         .ThenBy(r => r.DueDate);
            break;
    }

    var total = await query.CountAsync();

    var rawList = await query
        .Skip((page - 1) * pageSize)
        .Take(pageSize)
        .ToListAsync();

    var items = rawList.Select(r =>
    {
        // 🔥 Dùng trực tiếp luôn vì DueDate là DateTime?
        DateTime? due = r.DueDate;

        bool isOverdue = due != null && due < now;

        string color =
            due == null ? "gray"
            : due < now ? "red"
            : (due.Value - now).TotalHours <= 1 ? "orange"
            : "green";

        int? minutesToDue =
            due == null ? null : (int?)(due.Value - now).TotalMinutes;

        return new
        {
            r.Id,
            r.Title,
            r.Description,
            r.StatusId,
            r.SeverityId,
            r.FacilityId,
            r.CreatedAt,
            DueDate = due,
            IsOverdue = isOverdue,
            Color = color,
            MinutesToDue = minutesToDue
        };
    }).ToList();

    return Ok(new
    {
        items,
        total,
        page,
        pageSize,
        totalPages = (int)Math.Ceiling(total / (double)pageSize)
    });
}


        // =========================================================
        // 2) UPDATE STATUS
        // =========================================================
        [HttpPut("requests/{id:long}/status")]
        public async Task<IActionResult> UpdateStatus(long id, [FromBody] TechnicianUpdateStatusDto dto)
        {
            long techId = GetCurrentUserId();

            var req = await _db.requests
                .FirstOrDefaultAsync(r => r.Id == id && r.AssigneeId == techId);

            if (req == null)
                return NotFound(new { error = "Request not found or not assigned to you." });

            var validStatus = await _db.request_statuses.AnyAsync(s => s.Id == dto.StatusId);
            if (!validStatus)
                return BadRequest(new { error = "Invalid status." });

            // Completed needs image
            if (dto.StatusId == 3)
            {
                bool hasImage = await _db.attachments.AnyAsync(a => a.RequestId == id);
                if (!hasImage)
                    return BadRequest(new
                    {
                        error = "You must upload at least 1 verification image before marking Completed."
                    });
            }

            string beforeJson = req.StatusId.ToString();
            req.StatusId = dto.StatusId;
            req.UpdatedAt = DateTime.UtcNow;

            if (!string.IsNullOrWhiteSpace(dto.Note))
            {
                _db.request_comments.Add(new Request_Comments
                {
                    RequestId = id,
                    AuthorUserId = techId,
                    Body = dto.Note!,
                    AttachmentsCount = 0,
                    CreatedAt = DateTime.UtcNow
                });
            }

            _db.audit_logs.Add(new audit_logs
            {
                actor_user_id = techId,
                action = "Tech_UpdateStatus",
                entity = "Request",
                entity_id = id,
                before_json = beforeJson,
                after_json = dto.StatusId.ToString(),
                created_at = DateTime.UtcNow,
                ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                user_agent = Request.Headers["User-Agent"].ToString()
            });

            await _db.SaveChangesAsync();
            return Ok(new { message = "Status updated." });
        }

        // =========================================================
        // 3) UPLOAD IMAGE (Cloudinary)
        // =========================================================
        [HttpPost("requests/{id:long}/upload")]
        public async Task<IActionResult> Upload(long id, List<IFormFile> files)
        {
            long techId = GetCurrentUserId();

            var req = await _db.requests
                .FirstOrDefaultAsync(r => r.Id == id && r.AssigneeId == techId);

            if (req == null)
                return NotFound(new { error = "Request not found or not assigned to you." });

            if (files == null || files.Count == 0)
                return BadRequest(new { error = "No file uploaded." });

            var results = new List<object>();

            foreach (var file in files)
            {
                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(file.FileName, file.OpenReadStream()),
                    Folder = "ohd/requests"
                };

                var uploadResult = await _cloudinary.UploadAsync(uploadParams);

                if (uploadResult.Error != null)
                    return BadRequest(new { error = uploadResult.Error.Message });

                var att = new Attachment
                {
                    RequestId = id,
                    UploadedByUserId = techId,
                    FileName = file.FileName,
                    MimeType = file.ContentType,
                    FileSizeBytes = file.Length,
                    StorageUrl = uploadResult.SecureUrl.ToString(),
                    CreatedAt = DateTime.UtcNow
                };

                _db.attachments.Add(att);

                results.Add(new
                {
                    att.Id,
                    att.StorageUrl
                });
            }

            await _db.SaveChangesAsync();

            return Ok(new
            {
                message = "Files uploaded successfully.",
                files = results
            });
        }

        // =========================================================
        // 4) GET DETAIL
        // =========================================================
        [HttpGet("requests/{id:long}")]
        public async Task<IActionResult> GetDetail(long id)
        {
            long techId = GetCurrentUserId();

            var req = await _db.requests.AsNoTracking()
                .FirstOrDefaultAsync(r => r.Id == id && r.AssigneeId == techId);

            if (req == null)
                return NotFound(new { error = "Request not found." });

            var comments = await _db.request_comments
                .Where(c => c.RequestId == id)
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();

            var images = await _db.attachments
                .Where(a => a.RequestId == id)
                .Select(a => a.StorageUrl)
                .ToListAsync();

            return Ok(new
            {
                request = req,
                images,
                comments
            });
        }
    }
}
