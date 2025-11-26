using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Ohd.Data;
using Ohd.Entities;

namespace Ohd.Controllers.Admin
{
    [ApiController]
    [Route("api/admin/notifications")]
    [Authorize(Roles = "Admin")]
    public class AdminNotificationsController : ControllerBase
    {
        private readonly OhdDbContext _db;

        public AdminNotificationsController(OhdDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var data = await _db.notifications
                .OrderByDescending(n => n.created_at)
                .ToListAsync();

            return Ok(data);
        }

        [HttpPost("{requestId:long}/send")]
        public async Task<IActionResult> Send(long requestId)
        {
            var noti = new Notification
            {
                request_id = requestId,
                sent_to_user_id = 1, // Admin
                message = $"Request #{requestId} is overdue!",
                is_read = false,
                created_at = DateTime.UtcNow
            };

            await _db.notifications.AddAsync(noti);
            await _db.SaveChangesAsync();

            return Ok(new { message = "Notification sent" });
        }
    }
}