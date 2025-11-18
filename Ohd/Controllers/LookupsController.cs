using Microsoft.AspNetCore.Mvc;
using Ohd.Repositories.Interfaces;

namespace Ohd.Controllers
{
    [ApiController]
    [Route("api/lookups")]
    public class LookupsController : ControllerBase
    {
        private readonly ILookupRepository _lookups;

        public LookupsController(ILookupRepository lookups)
        {
            _lookups = lookups;
        }

        [HttpGet("request-statuses")]
        public async Task<IActionResult> GetRequestStatuses()
        {
            var data = await _lookups.GetRequestStatusesAsync();
            return Ok(data);
        }

        [HttpGet("severities")]
        public async Task<IActionResult> GetSeverities()
        {
            var data = await _lookups.GetSeveritiesAsync();
            return Ok(data);
        }

        [HttpGet("priorities")]
        public async Task<IActionResult> GetRequestPriorities()
        {
            var data = await _lookups.GetRequestPrioritiesAsync();
            return Ok(data);
        }

        [HttpGet("facilities")]
        public async Task<IActionResult> GetFacilities()
        {
            var data = await _lookups.GetFacilitiesAsync();
            return Ok(data);
        }

        [HttpGet("tags")]
        public async Task<IActionResult> GetTags()
        {
            var data = await _lookups.GetTagsAsync();
            return Ok(data);
        }

        [HttpGet("skills")]
        public async Task<IActionResult> GetSkills()
        {
            var data = await _lookups.GetSkillsAsync();
            return Ok(data);
        }

        [HttpGet("teams")]
        public async Task<IActionResult> GetTeams()
        {
            var data = await _lookups.GetTeamsAsync();
            return Ok(data);
        }

        [HttpGet("sla-policies")]
        public async Task<IActionResult> GetSlaPolicies()
        {
            var data = await _lookups.GetSlaPoliciesAsync();
            return Ok(data);
        }

        [HttpGet("maintenance-windows")]
        public async Task<IActionResult> GetMaintenanceWindows()
        {
            var data = await _lookups.GetMaintenanceWindowsAsync();
            return Ok(data);
        }

        [HttpGet("system-settings")]
        public async Task<IActionResult> GetSystemSettings()
        {
            var data = await _lookups.GetSystemSettingsAsync();
            return Ok(data);
        }
    }
}
