using Microsoft.AspNetCore.Mvc;
using Ohd.DTOs.Requests;
using Ohd.Services;
using Microsoft.AspNetCore.Authorization;
namespace Ohd.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class RequestController : ControllerBase
    {
        private readonly RequestService _service;

        public RequestController(RequestService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(long id)
        {
            var result = await _service.GetByIdAsync(id);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create(RequestCreateDto dto)
        {
            var created = await _service.CreateAsync(dto);
            return Ok(created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(long id, RequestUpdateDto dto)
        {
            var ok = await _service.UpdateAsync(id, dto);
            if (!ok) return NotFound();
            return Ok(new { message = "Updated" });
        }

        [HttpPut("{id}/status")]
        public async Task<IActionResult> ChangeStatus(long id, RequestChangeStatusDto dto)
        {
            var ok = await _service.ChangeStatusAsync(id, dto);
            if (!ok) return NotFound();
            return Ok(new { message = "Status changed" });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var ok = await _service.DeleteAsync(id);
            if (!ok) return NotFound();
            return Ok(new { message = "Deleted" });
        }
        [HttpGet("overdue/count")]
        public async Task<IActionResult> GetOverdueCount()
        {
            var count = await _service.CountOverdueAsync();
            return Ok(new { overdue = count });
        }
        [HttpGet("overdue/list")]
        public async Task<IActionResult> GetOverdueList()
        {
            var list = await _service.GetOverdueListAsync();
            return Ok(list);
        }

    }
}