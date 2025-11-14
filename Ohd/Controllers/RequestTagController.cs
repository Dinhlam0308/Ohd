using Microsoft.AspNetCore.Mvc;
using Ohd.DTOs.Requests;
using Ohd.Services;

namespace Ohd.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RequestTagController : ControllerBase
    {
        private readonly RequestTagService _service;

        public RequestTagController(RequestTagService service)
        {
            _service = service;
        }

        [HttpGet("by-request/{requestId:long}")]
        public async Task<IActionResult> GetByRequest(long requestId)
        {
            return Ok(await _service.GetByRequestAsync(requestId));
        }

        [HttpPost("add")]
        public async Task<IActionResult> Add(RequestTagAssignDto dto)
        {
            await _service.AddTagAsync(dto);
            return Ok(new { message = "Tag added" });
        }

        [HttpPost("remove")]
        public async Task<IActionResult> Remove(RequestTagAssignDto dto)
        {
            var ok = await _service.RemoveTagAsync(dto);
            if (!ok) return NotFound();

            return Ok(new { message = "Tag removed" });
        }
    }
}