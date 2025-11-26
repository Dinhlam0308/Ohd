using Microsoft.EntityFrameworkCore;
using Ohd.Data;
using Ohd.Entities;
using Ohd.Repositories.Interfaces;

namespace Ohd.Repositories.Implementations
{
    public class RequestRepository : IRequestRepository
    {
        private readonly OhdDbContext _context;

        public RequestRepository(OhdDbContext context)
        {
            _context = context;
        
        }

        public async Task<List<Request>> GetAllAsync()
        {
            return await _context.requests.AsNoTracking().ToListAsync();
        }

        public async Task<Request?> GetByIdAsync(long id)
        {
            return await _context.requests.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Request> CreateAsync(Request entity)
        {
            _context.requests.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> UpdateAsync(Request entity)
        {
            _context.requests.Update(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(long id)
        {
            var entity = await _context.requests.FindAsync(id);
            if (entity == null) return false;

            _context.requests.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task AddHistoryAsync(request_history history)
        {
            _context.request_history.Add(history);
            await _context.SaveChangesAsync();
        }
        public async Task<int> CountOverdueAsync()
        {
            var now = DateTime.UtcNow;

            return await _context.requests
                .Where(r => r.DueDate != null
                            && r.DueDate < now
                            && r.CompletedAt == null)
                .CountAsync();
        
        }
        public async Task<List<Request>> GetOverdueListAsync()
        {
            var now = DateTime.UtcNow;
            return await _context.requests
                .Where(r => r.DueDate != null 
                            && r.DueDate < now 
                            && r.CompletedAt == null)
                .ToListAsync();
        }

    }

}