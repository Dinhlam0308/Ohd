using Microsoft.EntityFrameworkCore;
using Ohd.Data;
using Ohd.DTOs.System;
using Ohd.Entities;

namespace Ohd.Services
{
    public class SystemSettingService
    {
        private readonly OhdDbContext _context;

        public SystemSettingService(OhdDbContext context)
        {
            _context = context;
        }

        public async Task<SystemSetting?> GetByKeyAsync(string key)
        {
            return await _context.system_settings.FindAsync(key);
        }

        public async Task<SystemSetting> UpsertAsync(SystemSettingDto dto)
        {
            var existing = await _context.system_settings.FindAsync(dto.Key);

            if (existing == null)
            {
                var entity = new SystemSetting
                {
                    key = dto.Key,
                    value_json = dto.ValueJson,
                    updated_at = DateTime.UtcNow
                };
                _context.system_settings.Add(entity);
                await _context.SaveChangesAsync();
                return entity;
            }
            else
            {
                existing.value_json = dto.ValueJson;
                existing.updated_at = DateTime.UtcNow;
                await _context.SaveChangesAsync();
                return existing;
            }
        }
    }
}