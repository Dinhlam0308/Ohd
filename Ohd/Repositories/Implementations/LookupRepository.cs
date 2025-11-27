using Microsoft.EntityFrameworkCore;
using Ohd.Data;
using Ohd.Entities;
using Ohd.Repositories.Interfaces;

namespace Ohd.Repositories.Implementations
{
    public class LookupRepository : ILookupRepository
    {
        private readonly OhdDbContext _context;

        public LookupRepository(OhdDbContext context)
        {
            _context = context;
        }

        public async Task<List<RequestStatus>> GetRequestStatusesAsync()
        {
            return await _context.request_statuses
                .OrderBy(x => x.Name)
                .ToListAsync();
        }

        public async Task<List<Severity>> GetSeveritiesAsync()
        {
            return await _context.severities
                .OrderBy(x => x.name)   // FIX: Không còn gọi sort_order
                .ToListAsync();
        }

        public async Task<List<RequestPriority>> GetRequestPrioritiesAsync()
        {
            return await _context.request_priorities
                .OrderBy(x => x.name)   // FIX
                .ToListAsync();
        }

        public async Task<List<Facility>> GetFacilitiesAsync()
        {
            return await _context.Facilities
                .OrderBy(x => x.Name)
                .ToListAsync();
        }

        public async Task<List<Tags>> GetTagsAsync()
        {
            return await _context.tags
                .OrderBy(x => x.name)
                .ToListAsync();
        }

        public async Task<List<Skills>> GetSkillsAsync()
        {
            return await _context.skills
                .OrderBy(x => x.name)
                .ToListAsync();
        }

        public async Task<List<Teams>> GetTeamsAsync()
        {
            return await _context.teams
                .OrderBy(x => x.team_name)
                .ToListAsync();
        }

        public async Task<List<sla_policies>> GetSlaPoliciesAsync()
        {
            return await _context.sla_policies
                .OrderBy(x => x.id)
                .ToListAsync();
        }

        public async Task<List<MaintenanceWindow>> GetMaintenanceWindowsAsync()
        {
            return await _context.maintenance_windows
                .OrderByDescending(x => x.start_time)
                .ToListAsync();
        }

        public async Task<List<SystemSetting>> GetSystemSettingsAsync()
        {
            return await _context.system_settings
                .OrderBy(x => x.key)
                .ToListAsync();
        }
    }
}
