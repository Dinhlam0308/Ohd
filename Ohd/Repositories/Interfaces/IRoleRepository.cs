using System.Threading.Tasks;

namespace Ohd.Repositories.Interfaces
{
    public interface IRoleRepository
    {
        Task AssignRoleToUser(long userId, int roleId);
        Task RemoveAllRolesFromUser(long userId);   // MUST MATCH EXACTLY
    }
}