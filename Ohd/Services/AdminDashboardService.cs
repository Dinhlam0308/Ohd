using Microsoft.EntityFrameworkCore;
using Ohd.Data;

namespace Ohd.Services
{
    public class AdminDashboardService
    {
        private readonly OhdDbContext _context;

        public AdminDashboardService(OhdDbContext context)
        {
            _context = context;
        }

        public async Task<object> GetDashboardAsync()
        {
            var totalUsers = await _context.Users.CountAsync();
            var activeUsers = await _context.Users.CountAsync(u => u.Is_Active);

            // ✔ DbSet đúng tên: requests
            var totalRequests = await _context.requests.CountAsync();

            // ✔ Open requests (StatusId = 1: NEW, 2: IN_PROGRESS)
            var openRequests = await _context.requests
                .CountAsync(r => r.StatusId == 1 || r.StatusId == 2);

            // ❗ Bạn không có Due_At, nên mình tính overdue = > 3 ngày mà chưa resolved/closed
            var limitDate = DateTime.UtcNow.AddDays(-3);

            var overdueRequests = await _context.requests
                .CountAsync(r =>
                    (r.StatusId == 1 || r.StatusId == 2) &&
                    r.CreatedAt <= limitDate
                );


            var totalFacilities = await _context.Facilities.CountAsync();

            return new
            {
                totalUsers,
                activeUsers,
                totalRequests,
                openRequests,
                overdueRequests,
                totalFacilities
            };
        }
    }
}