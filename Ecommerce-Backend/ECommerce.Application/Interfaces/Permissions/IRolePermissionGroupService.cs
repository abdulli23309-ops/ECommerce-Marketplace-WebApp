namespace ECommerce.Application.Interfaces
{
    public interface IRolePermissionGroupService
    {
        Task AssignGroupToRoleAsync(Guid roleId, Guid groupId);
        Task RemoveGroupFromRoleAsync(Guid roleId, Guid groupId);
        Task<IEnumerable<Guid>> GetGroupIdsForRoleAsync(Guid roleId);
    }
}