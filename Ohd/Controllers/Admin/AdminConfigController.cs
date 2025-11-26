using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ohd.DTOs.Admin;
using Ohd.DTOs.Roles;
using Ohd.DTOs.SystemSettings;
using Ohd.Services;
using Ohd.DTOs.References;
using Ohd.DTOs.Severity;
namespace Ohd.Controllers.Admin

{
    
    [Route("api/admin/config")]
    [Authorize(Roles = "Admin")]
    public class AdminConfigController : ControllerBase
    {
        private readonly AdminConfigService _service;

        public AdminConfigController(AdminConfigService service)
        {
            _service = service;
        }

        // ==========================
        // SEVERITIES
        // ==========================
        [HttpGet("severities")]
        public async Task<IActionResult> GetSeverities()
        {
            var data = await _service.GetSeveritiesAsync();
            return Ok(data);
        }

        [HttpPost("severities")]
        public async Task<IActionResult> CreateSeverity([FromBody] SeverityCreateDto dto)
        {
            var entity = await _service.CreateSeverityAsync(dto);
            return Ok(entity);
        }

        [HttpPut("severities/{id:int}")]
        public async Task<IActionResult> UpdateSeverity(int id, [FromBody] SeverityUpdateDto dto)
        {
            var ok = await _service.UpdateSeverityAsync(id, dto);
            if (!ok) return NotFound();
            return Ok(new { message = "Severity updated" });
        }

        [HttpDelete("severities/{id:int}")]
        public async Task<IActionResult> DeleteSeverity(int id)
        {
            var ok = await _service.DeleteSeverityAsync(id);
            if (!ok) return NotFound();
            return Ok(new { message = "Severity deleted" });
        }

        // ==========================
        // REQUEST STATUSES
        // ==========================

        // 🔹 Lấy tất cả (default)
        [HttpGet("statuses/all")]
        public async Task<IActionResult> GetAllStatuses()
        {
            var data = await _service.GetRequestStatusesAsync();
            return Ok(data);
        }

        // 🔹 Lọc theo search / isFinal / isOverdue
        [HttpGet("statuses")]
        public async Task<IActionResult> FilterStatuses(
            [FromQuery] string? search,
            [FromQuery] bool? isFinal,
            [FromQuery] bool? isOverdue)
        {
            var data = await _service.GetRequestStatusesAsync(search, isFinal, isOverdue);
            return Ok(data);
        }

        // 🔹 Tạo mới trạng thái
        [HttpPost("statuses")]
        public async Task<IActionResult> CreateRequestStatus([FromBody] RequestStatusCreateDto dto)
        {
            var created = await _service.CreateStatusAsync(dto);
            return Ok(created);
        }

        // 🔹 Cập nhật
        [HttpPut("statuses/{id:int}")]
        public async Task<IActionResult> UpdateRequestStatus(int id, [FromBody] RequestStatusUpdateDto dto)
        {
            var ok = await _service.UpdateStatusAsync(id, dto);
            if (!ok) return NotFound();

            return Ok(new { message = "Request status updated" });
        }

        // 🔹 Xoá trạng thái
        [HttpDelete("statuses/{id:int}")]
        public async Task<IActionResult> DeleteRequestStatus(int id)
        {
            var ok = await _service.DeleteStatusAsync(id);
            if (!ok) return NotFound();

            return Ok(new { message = "Request status deleted" });
        }

        // 🔹 Gửi thông báo cho toàn bộ request ở trạng thái này (nếu có IsOverdue)
        [HttpPost("statuses/{id:int}/send-overdue-notifications")]
        public async Task<IActionResult> SendOverdueNotifications(int id)
        {
            var count = await _service.SendOverdueNotificationsForStatusAsync(id);
            return Ok(new { message = "Notifications triggered", affectedRequests = count });
        }

        // ==========================
        // CATEGORIES
        // ==========================
        [HttpGet("categories")]
        public async Task<IActionResult> GetCategories()
        {
            var data = await _service.GetCategoriesAsync();
            return Ok(data);
        }

        [HttpPost("categories")]
        public async Task<IActionResult> CreateCategory([FromBody] CategoryCreateDto dto)
        {
            var entity = await _service.CreateCategoryAsync(dto);
            return Ok(entity);
        }

        [HttpPut("categories/{id:int}")]
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] CategoryUpdateDto dto)
        {
            var ok = await _service.UpdateCategoryAsync(id, dto);
            if (!ok) return NotFound();
            return Ok(new { message = "Category updated" });
        }

        [HttpDelete("categories/{id:int}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var ok = await _service.DeleteCategoryAsync(id);
            if (!ok) return NotFound();

            return Ok(new { message = "Category deleted" });
        }

        // ==========================
        // SYSTEM SETTINGS
        // ==========================
        [HttpGet("settings")]
        public async Task<IActionResult> GetSettings()
        {
            var data = await _service.GetSettingsAsync();
            return Ok(data);
        }

        [HttpGet("settings/{key}")]
        public async Task<IActionResult> GetSetting(string key)
        {
            var item = await _service.GetSettingAsync(key);
            if (item == null) return NotFound();
            return Ok(item);
        }

        [HttpPost("settings")]
        public async Task<IActionResult> UpsertSetting([FromBody] SystemSettingDto dto)
        {
            var item = await _service.UpsertSettingAsync(dto);
            return Ok(item);
        }

        [HttpDelete("settings/{key}")]
        public async Task<IActionResult> DeleteSetting(string key)
        {
            var ok = await _service.DeleteSettingAsync(key);
            if (!ok) return NotFound();

            return Ok(new { message = "Setting deleted" });
        }

        // ==========================
        // ROLES
        // ==========================
        [HttpGet("roles")]
        public async Task<IActionResult> GetRoles()
        {
            var data = await _service.GetRolesAsync();
            return Ok(data);
        }

        [HttpPost("roles")]
        public async Task<IActionResult> CreateRole([FromBody] RoleCreateDto dto)
        {
            var role = await _service.CreateRoleAsync(dto);
            return Ok(role);
        }

        [HttpPut("roles/{id:int}")]
        public async Task<IActionResult> UpdateRole(int id, [FromBody] RoleUpdateDto dto)
        {
            var ok = await _service.UpdateRoleAsync(id, dto);
            if (!ok) return NotFound();
            return Ok(new { message = "Role updated" });
        }

        [HttpDelete("roles/{id:int}")]
        public async Task<IActionResult> DeleteRole(int id)
        {
            var ok = await _service.DeleteRoleAsync(id);
            if (!ok) return NotFound();
            return Ok(new { message = "Role deleted" });
        }
    }
}