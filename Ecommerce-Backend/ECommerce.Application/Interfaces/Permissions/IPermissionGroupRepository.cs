using ECommerce.Application.Helpers;
using ECommerce.Domain.Entities;

namespace ECommerce.Application.Interfaces
{
    public interface IPermissionGroupRepository
    {
        Task<PagedResult<PermissionGroup>> GetPagedAsync(int page, int pageSize, string? search = null, string? sortBy = null);
        Task<PermissionGroup?> GetByIdAsync(Guid id);
        Task<bool> ExistsByNameAsync(string name, Guid? excludeId = null);
        Task<bool> HasPermissionsAsync(Guid groupId);
        Task AddAsync(PermissionGroup group);
        void Update(PermissionGroup group);
        void Delete(PermissionGroup group);
        Task AddPermissionToGroupAsync(Guid groupId, Guid permissionId);
        Task RemoveAllPermissionsFromGroupAsync(Guid groupId);
        Task ExecuteSqlRawAsync(string sql);

        Task RemovePermissionFromGroupAsync(Guid groupId, Guid permissionId);
        Task<IEnumerable<Guid>> GetPermissionIdsByGroupIdAsync(Guid groupId);
        Task SaveChangesAsync();
    }
}