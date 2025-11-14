using Microsoft.AspNetCore.Mvc;
using Ohd.DTOs.System;
using Ohd.Services;

namespace Ohd.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SystemSettingController : ControllerBase
    {
        private readonly SystemSettingService _service;

        public SystemSettingController(SystemSettingService service)
        {
            _service = service;
        }

        [HttpGet("{key}")]
        public async Task<IActionResult> Get(string key)
        {
            var item = await _service.GetByKeyAsync(key);
            if (item == null) return NotFound();
            return Ok(item);
        }

        [HttpPost]
        public async Task<IActionResult> Upsert(SystemSettingDto dto)
        {
            var item = await _service.UpsertAsync(dto);
            return Ok(item);
        }
    }
}