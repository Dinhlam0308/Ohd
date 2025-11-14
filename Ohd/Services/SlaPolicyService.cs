using Microsoft.EntityFrameworkCore;
using Ohd.Data;
using Ohd.DTOs.Sla;
using Ohd.Entities;

namespace Ohd.Services
{
    public class SlaPolicyService
    {
        private readonly OhdDbContext _context;

        public SlaPolicyService(OhdDbContext context)
        {
            _context = context;
        }

        public async Task<List<sla_policies>> GetAllAsync()
        {
            return await _context.sla_policies
                .OrderBy(x => x.priority)
                .ToListAsync();
        }

        public async Task<sla_policies?> GetByIdAsync(int id)
        {
            return await _context.sla_policies.FindAsync(id);
        }

        public async Task<sla_policies> CreateAsync(SlaPolicyCreateDto dto)
        {
            var entity = new sla_policies
            {
                facility_id = dto.FacilityId,
                category_id = dto.CategoryId,
                priority = dto.Priority,
                respond_within_mins = dto.RespondWithinMins,
                resolve_within_mins = dto.ResolveWithinMins,
                active = dto.Active,
                created_at = DateTime.UtcNow
            };

            _context.sla_policies.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> UpdateAsync(int id, SlaPolicyUpdateDto dto)
        {
            var entity = await _context.sla_policies.FindAsync(id);
            if (entity == null) return false;

            entity.priority = dto.Priority;
            entity.respond_within_mins = dto.RespondWithinMins;
            entity.resolve_within_mins = dto.ResolveWithinMins;
            entity.active = dto.Active;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _context.sla_policies.FindAsync(id);
            if (entity == null) return false;

            _context.sla_policies.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
