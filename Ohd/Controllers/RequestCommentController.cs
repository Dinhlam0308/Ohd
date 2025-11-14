using Microsoft.AspNetCore.Mvc;
using Ohd.DTOs.Requests;
using Ohd.Services;

namespace Ohd.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RequestCommentController : ControllerBase
    {
        private readonly RequestCommentService _service;

        public RequestCommentController(RequestCommentService service)
        {
            _service = service;
        }

        [HttpGet("by-request/{requestId:long}")]
        public async Task<IActionResult> GetByRequest(long requestId)
        {
            return Ok(await _service.GetByRequestAsync(requestId));
        }

        [HttpPost]
        public async Task<IActionResult> Create(RequestCommentCreateDto dto)
        {
            return Ok(await _service.CreateAsync(dto));
        }

        [HttpPut("{id:long}")]
        public async Task<IActionResult> Update(long id, RequestCommentUpdateDto dto)
        {
            var ok = await _service.UpdateAsync(id, dto);
            if (!ok) return NotFound();

            return Ok(new { message = "Updated" });
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