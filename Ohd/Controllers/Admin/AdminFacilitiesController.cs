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
        public async Task<IActionResult> GetAll()
        {
            var data = await _db.Facilities
                .OrderByDescending(f => f.CreatedAt)
                .ToListAsync();

            return Ok(data);
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