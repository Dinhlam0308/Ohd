using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Ohd.Data;

namespace Ohd.Controllers.Admin
{
    [ApiController]
    [Route("api/admin/requests")]
    [Authorize(Roles = "Admin")]
    public class AdminRequestsController : ControllerBase
    {
        private readonly OhdDbContext _db;

        public AdminRequestsController(OhdDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<IActionResult> GetRequests(int limit = 20)
        {
            try
            {
                var list = await _db.requests
                    .OrderByDescending(r => r.CreatedAt)
                    .Take(limit)
                    .ToListAsync();

                return Ok(list);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message, stack = ex.StackTrace });
            }
        }


        [HttpPut("{id:long}/assign/{assigneeId:long}")]
        public async Task<IActionResult> Assign(long id, long assigneeId)
        {
            var req = await _db.requests.FirstOrDefaultAsync(r => r.Id == id);
            if (req == null) return NotFound();

            req.AssigneeId = assigneeId;
            req.UpdatedAt = DateTime.UtcNow;

            await _db.SaveChangesAsync();
            return Ok(new { message = "Assigned successfully" });
        }

        [HttpPut("{id:long}/status/{statusId:int}")]
        public async Task<IActionResult> ChangeStatus(long id, int statusId)
        {
            var req = await _db.requests.FirstOrDefaultAsync(r => r.Id == id);
            if (req == null) return NotFound();

            req.StatusId = statusId;
            req.UpdatedAt = DateTime.UtcNow;

            await _db.SaveChangesAsync();
            return Ok(new { message = "Status updated" });
        }

        [HttpPut("{id:long}/close")]
        public async Task<IActionResult> Close(long id)
        {
            var req = await _db.requests.FirstOrDefaultAsync(r => r.Id == id);
            if (req == null) return NotFound();

            req.StatusId = 5; // Closed
            req.UpdatedAt = DateTime.UtcNow;

            await _db.SaveChangesAsync();
            return Ok(new { message = "Request closed" });
        }
    }
}
