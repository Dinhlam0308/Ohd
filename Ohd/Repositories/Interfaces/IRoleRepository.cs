namespace Ohd.Repositories.Interfaces
{
    public interface IRoleRepository
    {
        Task AssignRoleToUser(long userId, int roleId);
    }
}