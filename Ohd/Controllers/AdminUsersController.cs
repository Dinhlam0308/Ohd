using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ohd.DTOs.Admin;
using Ohd.Services;
using Ohd.DTOs.Common;
namespace Ohd.Controllers.Admin
{
    [ApiController]
    [Route("api/admin/users")]
    [Authorize(Roles = "Admin")]
    public class AdminUsersController : ControllerBase
    {
        private readonly AdminUserService _service;

        public AdminUsersController(AdminUserService service)
        {
            _service = service;
        }

        // GET ALL USERS
        // GET USERS WITH PAGINATION + SEARCH
        [HttpGet]
        public async Task<IActionResult> GetUsers(
            int page = 1,
            int pageSize = 10,
            string? search = ""
        )
        {
            if (page < 1) page = 1;
            if (pageSize <= 0) pageSize = 10;

            var (items, total) = await _service.GetUsersPagedAsync(search, page, pageSize);

            return Ok(new
            {
                items,
                total,
                page,
                pageSize,
                totalPages = (int)Math.Ceiling(total / (double)pageSize)
            });
        }

        // CREATE USER
        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] AdminCreateUserRequest req)
        {
            var (ok, error, temp) = await _service.CreateAsync(req);
            if (!ok) return BadRequest(new { error });

            return Ok(new
            {
                message = "User created successfully",
                tempPassword = temp
            });
        }

        // CHANGE ROLE
        [HttpPut("{userId:long}/role/{roleId:int}")]
        public async Task<IActionResult> ChangeRole(long userId, int roleId)
        {
            var (ok, error) = await _service.ChangeUserRoleAsync(userId, roleId);
            if (!ok) return BadRequest(new { error });

            return Ok(new { message = "Role updated" });
        }

        // TOGGLE ACTIVE
        [HttpPut("{userId:long}/toggle-active")]
        public async Task<IActionResult> ToggleActive(
            long userId,
            [FromBody] AdminToggleUserDto dto)
        {
            var ok = await _service.ToggleActiveAsync(userId, dto.IsActive);
            if (!ok) return NotFound();

            return Ok(new { message = "User status updated" });
        }

        // DELETE USER
        [HttpDelete("{userId:long}")]
        public async Task<IActionResult> Delete(long userId)
        {
            var ok = await _service.DeleteAsync(userId);
            if (!ok) return NotFound();

            return Ok(new { message = "User deleted" });
        }
        [HttpPost("import-excel-update")]
        public async Task<IActionResult> ImportExcelUpdate([FromForm] IFormFile file)
        {
            var result = await _service.ImportUsersFromExcelUpdateAsync(file);
            return Ok(result);
        } 
        [HttpGet("export-excel")]
        public async Task<IActionResult> ExportExcel()
        {
            Console.WriteLine(User.Identity.IsAuthenticated);
            Console.WriteLine(User.IsInRole("Admin"));
            var bytes = await _service.ExportUsersToExcelAsync();

            return File(
                bytes,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                "users.xlsx"
            );
        }
    }
}