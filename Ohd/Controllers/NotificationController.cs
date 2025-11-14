using Microsoft.AspNetCore.Mvc;
using Ohd.DTOs.Requests;
using Ohd.Services;

namespace Ohd.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotificationController : ControllerBase
    {
        private readonly NotificationService _service;

        public NotificationController(NotificationService service)
        {
            _service = service;
        }

        [HttpGet("by-user/{userId:long}")]
        public async Task<IActionResult> GetByUser(long userId)
        {
            return Ok(await _service.GetByUserAsync(userId));
        }

        [HttpPost]
        public async Task<IActionResult> Create(NotificationCreateDto dto)
        {
            return Ok(await _service.CreateAsync(dto));
        }

        [HttpPut("mark-read/{id:long}")]
        public async Task<IActionResult> MarkRead(long id)
        {
            var ok = await _service.MarkReadAsync(id);
            if (!ok) return NotFound();

            return Ok(new { message = "Marked as read" });
        }
    }
}