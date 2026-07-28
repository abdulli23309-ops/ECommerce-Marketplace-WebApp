using ECommerce.Application.Interfaces;

namespace ECommerce.Application.Services.Admin
{
    public class RolePermissionGroupService : IRolePermissionGroupService
    {
        private readonly IRolePermissionGroupRepository _repo;

        public RolePermissionGroupService(IRolePermissionGroupRepository repo) => _repo = repo;

        public async Task AssignGroupToRoleAsync(Guid roleId, Guid groupId)
        {
            await _repo.AddAsync(new Domain.Entities.RolePermissionGroup
            {
                RoleId = roleId,
                PermissionGroupId = groupId
            });
            await _repo.SaveChangesAsync();
        }

        public async Task RemoveGroupFromRoleAsync(Guid roleId, Guid groupId)
        {
            await _repo.RemoveAsync(roleId, groupId);
            await _repo.SaveChangesAsync();
        }

        public async Task<IEnumerable<Guid>> GetGroupIdsForRoleAsync(Guid roleId)
            => await _repo.GetGroupIdsByRoleIdAsync(roleId);
    }
}