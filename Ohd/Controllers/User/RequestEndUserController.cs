using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ohd.Dtos.Requests;
using Ohd.Services;
using System.Security.Claims;

namespace Ohd.Controllers.RequestEndUser
{
    [ApiController]
    [Route("api/enduser/requests")]
    [Authorize]
    public class RequestEndUserController : ControllerBase
    {
        private readonly IRequestEndUserService _service;

        public RequestEndUserController(IRequestEndUserService service)
        {
            _service = service;
        }

        // Helper lấy userId từ token
        private long GetCurrentUserId()
        {
            var id = User.FindFirstValue(ClaimTypes.NameIdentifier)
                ?? throw new Exception("UserId missing in token.");

            return long.Parse(id);
        }

        private string? GetUserEmail()
        {
            return User.FindFirstValue(ClaimTypes.Email);
        }

        // =====================================
        // 1) Tạo yêu cầu mới
        // =====================================
        [HttpPost]
        public async Task<IActionResult> CreateRequest(
            [FromBody] CreateRequestDto dto,
            CancellationToken ct)
        {
            var userId = GetCurrentUserId();
            var email = GetUserEmail();

            var newId = await _service.CreateRequestAsync(userId, email, dto, ct);

            return CreatedAtAction(nameof(GetMyRequestDetail),
                new { id = newId },
                new { id = newId });
        }

        // =====================================
        // 2) Lấy danh sách yêu cầu của chính mình
        // =====================================
        [HttpGet("my")]
        public async Task<IActionResult> GetMyRequests(CancellationToken ct)
        {
            var userId = GetCurrentUserId();
            var list = await _service.GetMyRequestsAsync(userId, ct);

            return Ok(list);
        }

        // =====================================
        // 3) Xem chi tiết yêu cầu
        // =====================================
        [HttpGet("{id:long}")]
        public async Task<IActionResult> GetMyRequestDetail(long id, CancellationToken ct)
        {
            var userId = GetCurrentUserId();
            var dto = await _service.GetMyRequestDetailAsync(userId, id, ct);

            if (dto == null) return NotFound();

            return Ok(dto);
        }

        // =====================================
        // 4) Đóng yêu cầu
        // =====================================
        [HttpPost("{id:long}/close")]
        public async Task<IActionResult> CloseRequest(
            long id,
            [FromBody] CloseRequestDto dto,
            CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(dto.Reason))
                return BadRequest("Reason is required.");

            var userId = GetCurrentUserId();
            var ok = await _service.CloseMyRequestAsync(userId, id, dto.Reason, ct);

            if (!ok) return NotFound();

            return NoContent();
        }

        // =====================================
        // 5) Comment vào yêu cầu
        // =====================================
        [HttpPost("{id:long}/comments")]
        public async Task<IActionResult> AddComment(
            long id,
            [FromBody] AddCommentDto dto,
            CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(dto.Body))
                return BadRequest("Comment body is required.");

            var userId = GetCurrentUserId();
            var commentId = await _service.AddCommentAsync(userId, id, dto.Body, ct);

            if (commentId == null) return NotFound();

            return Ok(new { id = commentId });
        }
    }
}
