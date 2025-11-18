using AutoMapper;
using Ohd.DTOs.Requests;
using Ohd.Entities;
using Ohd.Repositories.Interfaces;

namespace Ohd.Services
{
    public class RequestService
    {
        private readonly IRequestRepository _repo;
        private readonly IMapper _mapper;

        public RequestService(IRequestRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        // ============================
        // GET ALL
        // ============================
        public async Task<List<Request>> GetAllAsync()
        {
            return await _repo.GetAllAsync();
        }

        // ============================
        // GET BY ID
        // ============================
        public async Task<Request?> GetByIdAsync(long id)
        {
            return await _repo.GetByIdAsync(id);
        }

        // ============================
        // CREATE REQUEST
        // ============================
        public async Task<Request> CreateAsync(RequestCreateDto dto)
        {
            var entity = _mapper.Map<Request>(dto);

            // set thêm các field hệ thống
            entity.StatusId  = 1;                 // default = New
            entity.CreatedAt = DateTime.UtcNow;
            entity.UpdatedAt = DateTime.UtcNow;

            // repository sẽ tự SaveChanges bên trong
            return await _repo.CreateAsync(entity);
        }

        // ============================
        // UPDATE REQUEST
        // ============================
        public async Task<bool> UpdateAsync(long id, RequestUpdateDto dto)
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity == null) return false;

            _mapper.Map(dto, entity);
            entity.UpdatedAt = DateTime.UtcNow;

            // trả về true/false từ repo
            return await _repo.UpdateAsync(entity);
        }

        // ============================
        // CHANGE STATUS
        // ============================
        public async Task<bool> ChangeStatusAsync(long id, RequestChangeStatusDto dto)
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity == null) return false;

            var oldStatus = entity.StatusId;

            entity.StatusId  = dto.ToStatusId;
            entity.UpdatedAt = DateTime.UtcNow;

            var ok = await _repo.UpdateAsync(entity);
            if (!ok) return false;

            var history = new request_history
            {
                request_id         = id,
                from_status_id     = oldStatus,
                to_status_id       = dto.ToStatusId,
                changed_by_user_id = dto.ChangedByUserId,
                remarks            = dto.Remarks,
                changed_at         = DateTime.UtcNow
            };

            await _repo.AddHistoryAsync(history);

            return true;
        }

        // ============================
        // DELETE
        // ============================
        public async Task<bool> DeleteAsync(long id)
        {
            return await _repo.DeleteAsync(id);
        }
    }
}
