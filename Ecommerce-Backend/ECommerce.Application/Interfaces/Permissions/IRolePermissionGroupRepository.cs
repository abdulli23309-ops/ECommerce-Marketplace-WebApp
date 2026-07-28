using ECommerce.Domain.Entities;

namespace ECommerce.Application.Interfaces
{
    public interface IRolePermissionGroupRepository
    {
        Task<bool> ExistsAsync(Guid roleId, Guid groupId);
        Task AddAsync(RolePermissionGroup link);
        Task RemoveAsync(Guid roleId, Guid groupId);
        Task<IEnumerable<Guid>> GetGroupIdsByRoleIdAsync(Guid roleId);
        Task SaveChangesAsync();
    }
}