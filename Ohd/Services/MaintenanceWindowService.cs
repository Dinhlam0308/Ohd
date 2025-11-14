using Microsoft.EntityFrameworkCore;
using Ohd.Data;
using Ohd.DTOs.Maintenance;
using Ohd.Entities;

namespace Ohd.Services
{
    public class MaintenanceWindowService
    {
        private readonly OhdDbContext _context;

        public MaintenanceWindowService(OhdDbContext context)
        {
            _context = context;
        }

        public async Task<List<MaintenanceWindow>> GetAllAsync()
        {
            return await _context.maintenance_windows
                .OrderByDescending(x => x.start_time)
                .ToListAsync();
        }

        public async Task<MaintenanceWindow?> GetByIdAsync(long id)
        {
            return await _context.maintenance_windows.FindAsync(id);
        }

        public async Task<MaintenanceWindow> CreateAsync(MaintenanceWindowCreateDto dto)
        {
            var entity = new MaintenanceWindow
            {
                facility_id = dto.FacilityId,
                start_time = dto.StartTime,
                end_time = dto.EndTime,
                reason = dto.Reason,
                created_at = DateTime.UtcNow
            };

            _context.maintenance_windows.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> UpdateAsync(long id, MaintenanceWindowUpdateDto dto)
        {
            var entity = await _context.maintenance_windows.FindAsync(id);
            if (entity == null) return false;

            entity.start_time = dto.StartTime;
            entity.end_time = dto.EndTime;
            entity.reason = dto.Reason;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(long id)
        {
            var entity = await _context.maintenance_windows.FindAsync(id);
            if (entity == null) return false;

            _context.maintenance_windows.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
