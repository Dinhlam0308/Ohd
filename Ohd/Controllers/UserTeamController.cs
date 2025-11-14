using Microsoft.AspNetCore.Mvc;
using Ohd.DTOs.Teams;
using Ohd.Services;

namespace Ohd.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserTeamController : ControllerBase
    {
        private readonly UserTeamService _service;

        public UserTeamController(UserTeamService service)
        {
            _service = service;
        }

        [HttpGet("by-user/{userId:long}")]
        public async Task<IActionResult> GetByUser(long userId)
        {
            var list = await _service.GetByUserAsync(userId);
            return Ok(list);
        }

        [HttpPost("assign")]
        public async Task<IActionResult> Assign(UserTeamAssignDto dto)
        {
            var ok = await _service.AddUserToTeamAsync(dto);
            return Ok(new { ok });
        }

        [HttpPost("remove")]
        public async Task<IActionResult> Remove(UserTeamAssignDto dto)
        {
            var ok = await _service.RemoveUserFromTeamAsync(dto);
            if (!ok) return NotFound();
            return Ok(new { ok });
        }
    }
}