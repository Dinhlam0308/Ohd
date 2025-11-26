using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Ohd.Data;

namespace Ohd.Controllers.Admin
{
    [ApiController]
    [Route("api/admin/dashboard")]
    [Authorize(Roles = "Admin")]
    public class AdminDashboardController : ControllerBase
    {
        private readonly OhdDbContext _db;

        public AdminDashboardController(OhdDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<IActionResult> GetDashboard()
        {
            var totalUsers = await _db.Users.CountAsync();
            var activeUsers = await _db.Users.CountAsync(u => u.Is_Active == true);

            var totalRequests = await _db.requests.CountAsync();
            var overdueRequests = await _db.requests
                .Where(r => r.StatusId != 4 && r.StatusId != 5) // không closed/resolved
                .Where(r => EF.Functions.DateDiffDay(r.CreatedAt, DateTime.UtcNow) >= 3)
                .CountAsync();

            var totalFacilities = await _db.Facilities.CountAsync();

            var latestRequests = await _db.requests
                .OrderByDescending(r => r.CreatedAt)
                .Take(10)
                .ToListAsync();

            return Ok(new
            {
                totalUsers,
                activeUsers,
                totalRequests,
                overdueRequests,
                totalFacilities,
                latestRequests
            });
        }
    }
}