using Ohd.Entities;

namespace Ohd.Repositories.Interfaces
{
    public interface IRequestRepository
    {
        Task<List<Request>> GetAllAsync();
        Task<Request?> GetByIdAsync(long id);

        Task<Request> CreateAsync(Request entity);
        Task<bool> UpdateAsync(Request entity);
        Task<bool> DeleteAsync(long id);

        // Request History
        Task AddHistoryAsync(request_history history);
    }
}