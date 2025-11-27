using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Ohd.Data;
using System.Security.Claims;
using Ohd.DTOs.Roles.DepartmentHead;
using Ohd.Entities;

namespace Ohd.Controllers.DepartmentHead


{
    [ApiController]
    [Route("api/departmenthead")]
    public class DepartmentHeadController : ControllerBase
    {
        private readonly OhdDbContext _db;

        public DepartmentHeadController(OhdDbContext db)
        {
            _db = db;
        }

        // ================================
        // 🔐 UTIL — Get current DeptHead ID
        // ================================
        private long GetUserId()
        {
            var id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (id == null) throw new Exception("User not logged in");
            return long.Parse(id);
        }

        private async Task<int?> GetFacilityId(long userId)
        {
            return await _db.Facilities
                .Where(f => f.HeadUserId == userId)
                .Select(f => f.Id)
                .FirstOrDefaultAsync();
        }

        // ================================
        // 1️⃣ New Requests
        // ================================
        [HttpGet("requests/new")]
        public async Task<IActionResult> GetNewRequests()
        {
            var facilityId = await GetFacilityId(GetUserId());
            const int STATUS_NEW = 1;

            var list = await _db.requests
                .Where(r => r.StatusId == STATUS_NEW)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();

            return Ok(list);
        }

        // ================================
        // 2️⃣ Pending Requests
        // ================================
        [HttpGet("requests/pending")]
        public async Task<IActionResult> GetPendingRequests()
        {
            var facilityId = await GetFacilityId(GetUserId());
            const int STATUS_ASSIGNED = 2;

            var list = await _db.requests
                .Where(r => r.StatusId == STATUS_ASSIGNED)
                .OrderByDescending(r => r.UpdatedAt)
                .ToListAsync();

            return Ok(list);
        }

        // ================================
        // 3️⃣ Technician List
        // ================================
        [HttpGet("technicians")]
        public async Task<IActionResult> GetTechnicians()
        {
            int TECHNICIAN_ROLE_ID = 3;

            var technicians = await _db.user_roles
                .Where(ur => ur.role_id == TECHNICIAN_ROLE_ID)
                .Join(_db.Users,
                    ur => ur.user_id,
                    u => u.Id,
                    (ur, u) => u)
                .ToListAsync();

            return Ok(technicians);
        }

        // ================================
        // 4️⃣ Assign Request
        // ================================
        public class AssignDto
        {
            public long RequestId { get; set; }
            public long AssigneeId { get; set; }
        }

        [HttpPost("assign")]
        public async Task<IActionResult> Assign([FromBody] AssignDto dto)
        {
            var req = await _db.requests.FindAsync(dto.RequestId);
            if (req == null) return NotFound();

            req.AssigneeId = dto.AssigneeId;
            req.StatusId = 2;
            req.UpdatedAt = DateTime.UtcNow;

            await _db.SaveChangesAsync();
            return Ok(new { message = "Assigned successfully" });
        }

        // ================================
        // 5️⃣ Notifications
        // ================================
        [HttpGet("notifications")]
        public async Task<IActionResult> GetNotifications()
        {
            var userId = GetUserId();

            var noti = await _db.notifications
                .Where(n => n.sent_to_user_id == userId)
                .OrderByDescending(n => n.created_at)
                .ToListAsync();

            return Ok(noti);
        }

        [HttpPut("notifications/{id}/read")]
        public async Task<IActionResult> MarkRead(long id)
        {
            var n = await _db.notifications.FindAsync(id);
            if (n == null) return NotFound();

            n.is_read = true;
            await _db.SaveChangesAsync();

            return Ok();
        }

        // ================================
        // 6️⃣ Dashboard Summary
        // ================================
        [HttpGet("dashboard")]
        public async Task<IActionResult> Dashboard()
        {
            var facilityId = await GetFacilityId(GetUserId());
            const int NEW = 1;
            const int ASSIGNED = 2;
            const int COMPLETED = 4;

            var total = await _db.requests.CountAsync();
            var newReq = await _db.requests.CountAsync(r => r.FacilityId == facilityId && r.StatusId == NEW);
            var assigned = await _db.requests.CountAsync(r => r.FacilityId == facilityId && r.StatusId == ASSIGNED);
            var completed = await _db.requests.CountAsync(r => r.FacilityId == facilityId && r.StatusId == COMPLETED);

            return Ok(new { total, newReq, assigned, completed });
        }

        // ================================
        // 7️⃣ Monthly Report
        // ================================
        [HttpGet("report/monthly")]
        public async Task<IActionResult> MonthlyReport()
        {
            var facilityId = await GetFacilityId(GetUserId());

            var now = DateTime.UtcNow;
            var start = new DateTime(now.Year, now.Month, 1);

            const int COMPLETED = 4;

            var total = await _db.requests.CountAsync(r =>
                r.FacilityId == facilityId && r.CreatedAt >= start);

            var completed = await _db.requests.CountAsync(r =>
                r.FacilityId == facilityId && r.StatusId == COMPLETED && r.CreatedAt >= start);

            return Ok(new
            {
                total,
                completed,
                completionRate = total == 0 ? 0 : completed * 100 / total
            });
        }

        // ================================
        // 8️⃣ Request Detail
        // ================================
        [HttpGet("request/{id}")]
        public async Task<IActionResult> GetRequestDetail(long id)
        {
            var req = await _db.requests.FindAsync(id);
            if (req == null) return NotFound();

            return Ok(req);
        }

        // ================================
        // 9️⃣ Request Timeline
        // ================================
        [HttpGet("request/{id}/timeline")]
        public async Task<IActionResult> GetTimeline(long id)
        {
            var timeline = await _db.timeline
                .Where(t => t.request_id == id)
                .OrderByDescending(t => t.created_at)
                .ToListAsync();

            return Ok(timeline);
        }

        // ================================
        // 🔟 Workload per Technician
        // ================================
        [HttpGet("workload")]
        public async Task<IActionResult> GetWorkload()
        {
            var facilityId = await GetFacilityId(GetUserId());
            int TECHNICIAN_ROLE_ID = 3;

            // Lấy danh sách technician đúng role = 3
            var technicians = await _db.user_roles
                .Where(ur => ur.role_id == TECHNICIAN_ROLE_ID)
                .Join(_db.Users,
                    ur => ur.user_id,
                    u => u.Id,
                    (ur, u) => u)
                .ToListAsync();

            var list = new List<object>();

            foreach (var t in technicians)
            {
                var newCount = await _db.requests.CountAsync(r =>
                    r.AssigneeId == t.Id &&
                    r.StatusId == 1 &&
                    r.FacilityId == facilityId);

                var inProgressCount = await _db.requests.CountAsync(r =>
                    r.AssigneeId == t.Id &&
                    r.StatusId == 2 &&
                    r.FacilityId == facilityId);

                var completedCount = await _db.requests.CountAsync(r =>
                    r.AssigneeId == t.Id &&
                    r.StatusId == 4 &&
                    r.FacilityId == facilityId);

                var overdueCount = await _db.requests.CountAsync(r =>
                    r.AssigneeId == t.Id &&
                    r.FacilityId == facilityId &&
                    r.StatusId != 4 &&
                    r.DueDate < DateTime.UtcNow);

                list.Add(new
                {
                    t.Id,
                    name = t.Username ?? t.Email ?? ("Technician #" + t.Id),
                    newCount,
                    inProgressCount,
                    completedCount,
                    overdueCount
                });
            }

            return Ok(list);
        }


        // ================================
        // 1️⃣1️⃣ SLA overview
        // ================================
        [HttpGet("sla")]
        public async Task<IActionResult> GetSLA()
        {
            var facilityId = await GetFacilityId(GetUserId());
            var now = DateTime.UtcNow;

            var total = await _db.requests.CountAsync(r => r.FacilityId == facilityId);

            var onTime = await _db.requests.CountAsync(r =>
                r.FacilityId == facilityId &&
                (r.DueDate == null || r.CompletedAt <= r.DueDate));

            var overdue = await _db.requests.CountAsync(r =>
                r.FacilityId == facilityId &&
                (r.CompletedAt > r.DueDate));

            return Ok(new
            {
                total,
                onTime,
                overdue,
                complianceRate = total == 0 ? 0 : (onTime * 100 / total)
            });
        }

        // ================================
        // 1️⃣2️⃣ Activity Log
        // ================================
        [HttpGet("activity")]
        public async Task<IActionResult> ActivityLog()
        {
            var facilityId = await GetFacilityId(GetUserId());

            var logs = await _db.timeline
                .Join(_db.requests,
                    t => t.request_id,
                    r => r.Id,
                    (t, r) => new { t, r })
                .Where(x => x.r.FacilityId == facilityId)
                .OrderByDescending(x => x.t.created_at)
                .Select(x => new
                {
                    x.t.request_id,
                    x.t.title,
                    x.t.note,
                    x.t.created_at,
                    x.t.user_name,
                    x.r.StatusId
                })
                .ToListAsync();

            return Ok(logs);
        }

        // ================================
        // 1️⃣3️⃣ Performance
        // ================================
        [HttpGet("performance")]
        public async Task<IActionResult> Performance()
        {
            var facilityId = await GetFacilityId(GetUserId());
            const int COMPLETED = 4;

            // Lấy danh sách technician thuộc facility này
            int TECHNICIAN_ROLE_ID = 3;

            var technicians = await _db.user_roles
                .Where(ur => ur.role_id == TECHNICIAN_ROLE_ID)
                .Join(_db.Users,
                    ur => ur.user_id,
                    u => u.Id,
                    (ur, u) => u)
                .ToListAsync();

            var list = new List<object>();

            foreach (var t in technicians)
            {
                // Completed
                var completed = await _db.requests.CountAsync(r =>
                    r.AssigneeId == t.Id &&
                    r.StatusId == COMPLETED &&
                    r.FacilityId == facilityId);

                // Overdue
                var overdue = await _db.requests.CountAsync(r =>
                    r.AssigneeId == t.Id &&
                    r.FacilityId == facilityId &&
                    r.StatusId != COMPLETED &&
                    r.DueDate < DateTime.UtcNow);

                // Avg Resolve Hours — phải check có dữ liệu
                var completedList = await _db.requests
                    .Where(r =>
                        r.AssigneeId == t.Id &&
                        r.FacilityId == facilityId &&
                        r.CompletedAt != null &&
                        r.CreatedAt != null)
                    .ToListAsync();

                double avgHours = 0;

                if (completedList.Count > 0)
                {
                    avgHours = completedList
                        .Select(r => (r.CompletedAt.Value - r.CreatedAt).TotalHours)
                        .Average();
                }

                list.Add(new
                {
                    t.Id,
                    t.Username,
                    completed,
                    overdue,
                    avgResolutionHours = Math.Round(avgHours, 2)
                });
            }

            return Ok(list);
        }


        // ================================
        // 1️⃣4️⃣ Bulk Assign
        // ================================
        [HttpGet("bulk/requests")]
        public async Task<IActionResult> BulkRequests()
        {
            var facilityId = await GetFacilityId(GetUserId());
            const int NEW = 1;

            var list = await _db.requests
                .Where(r => r.FacilityId == facilityId && r.StatusId == NEW)
                .OrderBy(r => r.CreatedAt)
                .ToListAsync();

            return Ok(list);
        }

        public class BulkAssignDto
        {
            public List<long> RequestIds { get; set; }
            public long AssigneeId { get; set; }
        }

        [HttpPost("bulk/assign")]
        public async Task<IActionResult> BulkAssign([FromBody] BulkAssignDto dto)
        {
            foreach (var id in dto.RequestIds)
            {
                var req = await _db.requests.FindAsync(id);
                if (req != null)
                {
                    req.AssigneeId = dto.AssigneeId;
                    req.StatusId = 2;
                    req.UpdatedAt = DateTime.UtcNow;
                }
            }

            await _db.SaveChangesAsync();
            return Ok(new { message = "Bulk assign successful" });
        }

        // ================================
        // 1️⃣5️⃣ Export (placeholder)
        // ================================
        [HttpPost("export")]
        public IActionResult Export()
        {
            return Ok("Export functionality placeholder");
        }
        [HttpPut("request/reassign")]
        public async Task<IActionResult> Reassign([FromBody] AssignDto dto)
        {
            var req = await _db.requests.FindAsync(dto.RequestId);
            if (req == null) return NotFound("Request not found");

            req.AssigneeId = dto.AssigneeId;
            req.StatusId = 2;
            req.UpdatedAt = DateTime.UtcNow;

            await _db.SaveChangesAsync();

            return Ok(new { message = "Reassigned successfully" });
        }
        [HttpPut("request/{id}/close")]
        public async Task<IActionResult> CloseRequest(long id)
        {
            var req = await _db.requests.FindAsync(id);
            if (req == null) return NotFound();

            req.StatusId = 4; // Completed
            req.CompletedAt = DateTime.UtcNow;

            await _db.SaveChangesAsync();
            return Ok(new { message = "Request closed" });
        }
        [HttpPut("request/{id}/reopen")]
        public async Task<IActionResult> ReopenRequest(long id)
        {
            var req = await _db.requests.FindAsync(id);
            if (req == null) return NotFound();

            req.StatusId = 2; // Assigned lại
            req.UpdatedAt = DateTime.UtcNow;

            await _db.SaveChangesAsync();
            return Ok(new { message = "Request reopened" });
        }
        [HttpGet("requests/search")]
        public async Task<IActionResult> SearchRequests([FromQuery] RequestSearchDto dto)
        {
            var query = _db.requests.AsQueryable();

            if (dto.StatusId != null)
                query = query.Where(r => r.StatusId == dto.StatusId);

            if (dto.Priority != null)
                query = query.Where(r => r.PriorityId == dto.Priority);


            if (dto.AssigneeId != null)
                query = query.Where(r => r.AssigneeId == dto.AssigneeId);

            if (dto.FromDate != null)
                query = query.Where(r => r.CreatedAt >= dto.FromDate);

            if (dto.ToDate != null)
                query = query.Where(r => r.CreatedAt <= dto.ToDate);

            return Ok(await query.OrderByDescending(r => r.CreatedAt).ToListAsync());
        }
        public class CommentDto
        {
            public string Message { get; set; }
            public bool IsInternal { get; set; }
        }

        [HttpPost("request/{id}/comment")]
        public async Task<IActionResult> AddComment(long id, [FromBody] CommentDto dto)
        {
            var req = await _db.requests.FindAsync(id);
            if (req == null) return NotFound();

            var log = new Timeline
            {
                request_id = id,
                title = "Comment added",
                note = dto.Message,
                created_at = DateTime.UtcNow,
                user_name = User.Identity?.Name ?? "System"
                // ⚠️ Không dùng is_internal vì entity không có field này
            };

            _db.timeline.Add(log);
            await _db.SaveChangesAsync();

            return Ok(new { message = "Comment added" });
        }

        [HttpGet("technician/{id}/stats")]
        public async Task<IActionResult> GetTechnicianStats(long id)
        {
            var completed = await _db.requests.CountAsync(r => r.AssigneeId == id && r.StatusId == 4);
            var assigned  = await _db.requests.CountAsync(r => r.AssigneeId == id && r.StatusId == 2);
            var overdue   = await _db.requests.CountAsync(r => r.AssigneeId == id && r.DueDate < DateTime.UtcNow);

            return Ok(new { completed, assigned, overdue });
        }
// ================================
// 1️⃣6️⃣ Technicians Available in Schedule
// ================================
        [HttpGet("technicians/available")]
        public async Task<IActionResult> GetAvailableTechnicians(long requestId, DateTime dateTime)
        {
            int TECHNICIAN_ROLE_ID = 3;

            // Lấy facility của Department Head
            var facilityId = await GetFacilityId(GetUserId());

            // Lấy danh sách technician thuộc facility
            var technicians = await _db.user_roles
                .Where(ur => ur.role_id == TECHNICIAN_ROLE_ID)
                .Join(_db.Users,
                    ur => ur.user_id,
                    u => u.Id,
                    (ur, u) => u)
                .ToListAsync();

            // Range ± 1 giờ để tránh trùng lịch
            var startRange = dateTime.AddHours(-1);
            var endRange = dateTime.AddHours(1);

            // Danh sách kỹ thuật viên đang bận
            var busy = await _db.requests
                .Where(r =>
                    r.FacilityId == facilityId &&
                    r.AssigneeId != null &&
                    r.StatusId == 2 && // Assigned / In-progress
                    r.CreatedAt >= startRange &&
                    r.CreatedAt <= endRange)
                .Select(r => r.AssigneeId.Value)
                .ToListAsync();

            // Lọc technician rảnh
            var available = technicians
                .Where(t => !busy.Contains(t.Id))
                .Select(t => new
                {
                    t.Id,
                    t.Username,
                    t.Email
                })
                .ToList();

            return Ok(available);
        }

// ================================
// 1️⃣7️⃣ Smart Suggestion — Best Technician for the Request
// ================================
       [HttpGet("technicians/best")]
public async Task<IActionResult> GetBestTechnician(long requestId)
{
    var req = await _db.requests.FindAsync(requestId);
    if (req == null) return NotFound("Request not found");

    const int TECHNICIAN_ROLE_ID = 3;
    var facilityId = await GetFacilityId(GetUserId());

    // Lấy tất cả technicians trong cơ sở này
    var technicians = await _db.user_roles
        .Where(ur => ur.role_id == TECHNICIAN_ROLE_ID)
        .Join(_db.Users,
            ur => ur.user_id,
            u => u.Id,
            (ur, u) => u)
        .ToListAsync();

    var scoredList = new List<object>();

    foreach (var t in technicians)
    {
        // Số ticket đang xử lý
        var workload = await _db.requests.CountAsync(r =>
            r.AssigneeId == t.Id &&
            r.FacilityId == facilityId &&
            r.StatusId == 2);

        // Số ticket overdue
        var overdue = await _db.requests.CountAsync(r =>
            r.AssigneeId == t.Id &&
            r.FacilityId == facilityId &&
            r.StatusId != 4 &&
            r.DueDate < DateTime.UtcNow);

        // Trùng lịch trong ± 60 phút
        var conflict = await _db.requests.AnyAsync(r =>
            r.AssigneeId == t.Id &&
            r.FacilityId == facilityId &&
            r.StatusId == 2 &&
            Math.Abs((r.CreatedAt - req.CreatedAt).TotalMinutes) < 60);

        // Tính điểm
        double score = 0;
        if (!conflict) score += 50;
        score += Math.Max(0, 30 - workload * 10);
        score += Math.Max(0, 20 - overdue * 10);

        scoredList.Add(new
        {
            TechnicianId = t.Id,
            Name = t.Username ?? t.Email,
            Workload = workload,
            Overdue = overdue,
            Conflict = conflict,
            Score = score
        });
    }

    // Chọn thằng điểm cao nhất
    var best = scoredList
        .OrderByDescending(x => x.GetType().GetProperty("Score")?.GetValue(x))
        .FirstOrDefault();

    // Nếu null → return 404
    if (best == null)
        return NotFound("No technician available");

    return Ok(best);     // <<== QUAN TRỌNG
}


    }
}
