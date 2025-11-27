using Microsoft.EntityFrameworkCore;
using Ohd.Data;
using Ohd.DTOs.References;
using Ohd.Entities;
using Ohd.DTOs.Roles;
using Ohd.DTOs.SystemSettings;
using Ohd.DTOs.Severity;
namespace Ohd.Services

{
    public class AdminConfigService
    {
        private readonly OhdDbContext _db;

        public AdminConfigService(OhdDbContext db)
        {
            _db = db;
        }

        // =====================================================
        // 🔸 SEVERITIES
        // =====================================================
      public async Task<List<Severity>> GetSeveritiesAsync()
{
    return await _db.severities
        .OrderBy(s => s.sort_order)
        .ToListAsync();
}

public async Task<Severity?> GetSeverityAsync(int id)
{
    return await _db.severities
        .FirstOrDefaultAsync(s => s.id == id);
}

public async Task<Severity> CreateSeverityAsync(SeverityCreateDto dto)
{
    var sev = new Severity
    {
        code = dto.Code,
        name = dto.Name,
        sort_order = dto.SortOrder,
        auto_notify = dto.AutoNotify,
        notify_email_template_code = dto.NotifyEmailTemplateCode,
        created_at = DateTime.UtcNow
    };

    _db.severities.Add(sev);
    await _db.SaveChangesAsync();

    return sev;
}

public async Task<bool> UpdateSeverityAsync(int id, SeverityUpdateDto dto)
{
    var sev = await _db.severities.FindAsync(id);
    if (sev == null) return false;
    sev.name = dto.Name;
    sev.sort_order = dto.SortOrder;
    sev.auto_notify = dto.AutoNotify;
    sev.notify_email_template_code = dto.NotifyEmailTemplateCode;
    sev.updated_at = DateTime.UtcNow;

    await _db.SaveChangesAsync();
    return true;
}


public async Task<bool> DeleteSeverityAsync(int id)
{
    var entity = await _db.severities.FirstOrDefaultAsync(s => s.id == id);
    if (entity == null) return false;

    // (Optional) 🔥 Check xem Severity này có đang được dùng trong Request hay không
    bool isUsed = await _db.requests.AnyAsync(r => r.SeverityId == id);
    if (isUsed)
        throw new InvalidOperationException("Cannot delete severity because it is being used in requests.");

    _db.severities.Remove(entity);
    await _db.SaveChangesAsync();
    return true;
}

        // =====================================================
        // 🔸 REQUEST STATUSES (trạng thái & overdue)
        // =====================================================
        public async Task<List<RequestStatus>> GetRequestStatusesAsync(
            string? search = null,
            bool? isFinal = null,
            bool? isOverdue = null)
        {
            var query = _db.request_statuses.AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(x =>
                    x.Code.Contains(search) || x.Name.Contains(search));
            }

            if (isFinal.HasValue)
            {
                query = query.Where(x => x.IsFinal == isFinal.Value);
            }

            if (isOverdue.HasValue)
            {
                query = query.Where(x => x.IsOverdue == isOverdue.Value);
            }

            return await query
                .OrderBy(x => x.Id)
                .ToListAsync();
        }

        public async Task<RequestStatus?> CreateStatusAsync(RequestStatusCreateDto dto)
        {
            // Không cho trùng Code
            bool codeExists = await _db.request_statuses
                .AnyAsync(x => x.Code == dto.Code);
            if (codeExists)
            {
                throw new InvalidOperationException("Status code already exists.");
            }

            var entity = new RequestStatus
            {
                Code = dto.Code,
                Name = dto.Name,
                IsFinal = dto.IsFinal,
                IsOverdue = dto.IsOverdue,
                Color = dto.Color,
                CreatedAt = DateTime.UtcNow
            };

            _db.request_statuses.Add(entity);
            await _db.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> UpdateStatusAsync(int id, RequestStatusUpdateDto dto)
        {
            var st = await _db.request_statuses.FindAsync(id);
            if (st == null) return false;

            st.Name = dto.Name;
            st.IsFinal = dto.IsFinal;
            st.IsOverdue = dto.IsOverdue;
            st.Color = dto.Color;

            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteStatusAsync(int id)
        {
            var st = await _db.request_statuses.FindAsync(id);
            if (st == null) return false;

            _db.request_statuses.Remove(st);
            await _db.SaveChangesAsync();
            return true;
        }

        // Gửi thông báo cho các request ở trạng thái quá hạn này
        public async Task<int> SendOverdueNotificationsForStatusAsync(int statusId)
        {
            var status = await _db.request_statuses.FindAsync(statusId);
            if (status == null || !status.IsOverdue)
                return 0;

            var overdueRequests = await _db.requests
                .Where(r => r.StatusId == statusId)
                .ToListAsync();

            // TODO: ở đây tuỳ bạn implement thêm Notification / Email
            // mình chỉ return số lượng để FE show
            return overdueRequests.Count;
        }

        // =====================================================
        // 🔸 CATEGORIES
        // =====================================================
        // ==========================
// 🔸 CATEGORIES
// ==========================
        public async Task<List<Category>> GetCategoriesAsync()
        {
            return await _db.categories
                .OrderBy(c => c.name)
                .ToListAsync();
        }

        public async Task<Category?> GetCategoryAsync(int id)
        {
            return await _db.categories.FirstOrDefaultAsync(c => c.id == id);
        }

        public async Task<Category> CreateCategoryAsync(CategoryCreateDto dto)
        {
            var entity = new Category
            {
                name = dto.Name,
                parent_id = dto.ParentId,
                created_at = DateTime.UtcNow
            };

            _db.categories.Add(entity);
            await _db.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> UpdateCategoryAsync(int id, CategoryUpdateDto dto)
        {
            var entity = await _db.categories.FirstOrDefaultAsync(c => c.id == id);
            if (entity == null) return false;

            entity.name = dto.Name;
            entity.parent_id = dto.ParentId;

            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteCategoryAsync(int id)
        {
            var entity = await _db.categories.FirstOrDefaultAsync(c => c.id == id);
            if (entity == null) return false;

            _db.categories.Remove(entity);
            await _db.SaveChangesAsync();
            return true;
        }

        // =====================================================
        // 🔸 SYSTEM SETTINGS
        // =====================================================
        public async Task<List<SystemSetting>> GetSettingsAsync()
        {
            return await _db.system_settings
                .OrderBy(s => s.key)
                .ToListAsync();
        }

        public async Task<SystemSetting?> GetSettingAsync(string key)
        {
            return await _db.system_settings.FirstOrDefaultAsync(s => s.key == key);
        }

        public async Task<SystemSetting> UpsertSettingAsync(SystemSettingDto dto)
        {
            var entity = await _db.system_settings.FirstOrDefaultAsync(s => s.key == dto.Key);
            if (entity == null)
            {
                entity = new SystemSetting
                {
                    key = dto.Key,
                    value_json = dto.Value,   // bạn lưu JSON text
                };
                _db.system_settings.Add(entity);
            }
            else
            {
                entity.value_json = dto.Value;
            }

            await _db.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> DeleteSettingAsync(string key)
        {
            var entity = await _db.system_settings.FirstOrDefaultAsync(s => s.key == key);
            if (entity == null) return false;

            _db.system_settings.Remove(entity);
            await _db.SaveChangesAsync();
            return true;
        }

        // =====================================================
        // 🔸 ROLES
        // =====================================================
        public async Task<List<Role>> GetRolesAsync()
        {
            return await _db.roles
                .OrderBy(r => r.id)
                .ToListAsync();
        }

        public async Task<Role?> CreateRoleAsync(RoleCreateDto dto)
        {
            bool nameExists = await _db.roles.AnyAsync(r => r.name == dto.Name);
            if (nameExists)
            {
                throw new InvalidOperationException("Role name already exists.");
            }

            var role = new Role
            {
                name = dto.Name,
                description = dto.Description
            };

            _db.roles.Add(role);
            await _db.SaveChangesAsync();
            return role;
        }

        public async Task<bool> UpdateRoleAsync(int id, RoleUpdateDto dto)
        {
            var role = await _db.roles.FindAsync(id);
            if (role == null) return false;

            bool nameExists = await _db.roles
                .AnyAsync(r => r.id != id && r.name == dto.Name);
            if (nameExists)
            {
                throw new InvalidOperationException("Role name already exists.");
            }

            role.name = dto.Name;
            role.description = dto.Description;

            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteRoleAsync(int id)
        {
            var role = await _db.roles.FindAsync(id);
            if (role == null) return false;

            _db.roles.Remove(role);
            await _db.SaveChangesAsync();
            return true;
        }
    }
}