using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Ohd.Data;
using Ohd.Entities;
using Ohd.Services;

namespace Ohd.Controllers.Admin
{
    [ApiController]
    [Route("api/admin/facilities")]
    [Authorize(Roles = "Admin")]
    public class AdminFacilitiesController : ControllerBase
    {
        private readonly OhdDbContext _db;
        private readonly AdminUserService _adminUserService;

        public AdminFacilitiesController(OhdDbContext db, AdminUserService adminUserService)
        {
            _db = db;
            _adminUserService = adminUserService;
        }

    
        [HttpGet]
        public async Task<IActionResult> GetFacilities(
            int page = 1,
            int pageSize = 10,
            string? search = ""
        )
        {
            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 10;

            var query = _db.Facilities.AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.Trim();
                query = query.Where(f => f.Name.Contains(search));
            }

            var total = await query.CountAsync();

            var items = await query
                .OrderByDescending(f => f.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(f => new {
                    f.Id,
                    f.Name,
                    f.Description,
                    f.CreatedAt,
                    f.HeadUserId
                })
                .ToListAsync();

            return Ok(new
            {
                items,
                total,
                page,
                pageSize,
                totalPages = (int)Math.Ceiling(total / (double)pageSize)
            });
        }

        [HttpPost]
        public async Task<IActionResult> Create(Facility fac)
        {
            fac.CreatedAt = DateTime.UtcNow;

            await _db.Facilities.AddAsync(fac);
            await _db.SaveChangesAsync();

            return Ok(new { message = "Created", fac });
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, Facility req)
        {
            var fac = await _db.Facilities.FirstOrDefaultAsync(x => x.Id == id);
            if (fac == null) return NotFound();

            fac.Name = req.Name;
            fac.Description = req.Description;
            fac.HeadUserId = req.HeadUserId;

            await _db.SaveChangesAsync();
            return Ok(new { message = "Updated" });
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var fac = await _db.Facilities.FirstOrDefaultAsync(x => x.Id == id);
            if (fac == null) return NotFound();

            _db.Facilities.Remove(fac);
            await _db.SaveChangesAsync();

            return Ok(new { message = "Deleted" });
        }

        [HttpGet("technicians")]
        public async Task<IActionResult> GetTechnicians()
        {
            var users = await _adminUserService.GetUsersByRole("Technician");
            return Ok(users);
        }

    }
}