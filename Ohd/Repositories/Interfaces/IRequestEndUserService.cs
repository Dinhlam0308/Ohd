using Ohd.Dtos.Requests;

namespace Ohd.Services
{
    public interface IRequestEndUserService
    {
        Task<long> CreateRequestAsync(
            long userId,
            string? userEmail,
            CreateRequestDto dto,
            CancellationToken ct = default);

        Task<IEnumerable<RequestListItemDto>> GetMyRequestsAsync(
            long userId,
            CancellationToken ct = default);

        Task<RequestDetailDto?> GetMyRequestDetailAsync(
            long userId,
            long requestId,
            CancellationToken ct = default);

        Task<bool> CloseMyRequestAsync(
            long userId,
            long requestId,
            string reason,
            CancellationToken ct = default);

        Task<long?> AddCommentAsync(
            long userId,
            long requestId,
            string body,
            CancellationToken ct = default);
    }
}