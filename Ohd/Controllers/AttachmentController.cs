using Microsoft.AspNetCore.Mvc;
using Ohd.DTOs.Requests;
using Ohd.Services;

namespace Ohd.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AttachmentController : ControllerBase
    {
        private readonly AttachmentService _service;

        public AttachmentController(AttachmentService service)
        {
            _service = service;
        }

        [HttpGet("by-request/{requestId:long}")]
        public async Task<IActionResult> GetByRequest(long requestId)
        {
            return Ok(await _service.GetByRequestAsync(requestId));
        }

        [HttpPost]
        public async Task<IActionResult> Create(AttachmentCreateDto dto)
        {
            return Ok(await _service.CreateAsync(dto));
        }

        [HttpDelete("{id:long}")]
        public async Task<IActionResult> Delete(long id)
        {
            var ok = await _service.DeleteAsync(id);
            if (!ok) return NotFound();

            return Ok(new { message = "Deleted" });
        }
    }
}