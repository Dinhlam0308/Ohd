using Ohd.Entities;

namespace Ohd.Repositories.Interfaces
{
    public interface ILookupRepository
    {
        // ==== Request-related lookups ====
        Task<List<RequestStatus>> GetRequestStatusesAsync();
        Task<List<Severity>> GetSeveritiesAsync();
        Task<List<RequestPriority>> GetRequestPrioritiesAsync();
        Task<List<Facility>> GetFacilitiesAsync();
        Task<List<Tags>> GetTagsAsync();

        // ==== Các danh mục khác ====
        Task<List<Skills>> GetSkillsAsync();
        Task<List<Teams>> GetTeamsAsync();
        Task<List<sla_policies>> GetSlaPoliciesAsync();
        Task<List<MaintenanceWindow>> GetMaintenanceWindowsAsync();
        Task<List<SystemSetting>> GetSystemSettingsAsync();
    }
}