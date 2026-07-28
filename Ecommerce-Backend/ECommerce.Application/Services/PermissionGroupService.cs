using ECommerce.Application.DTOs.Admin;
using ECommerce.Application.Helpers;
using ECommerce.Application.Interfaces;
using ECommerce.Domain.Entities;

namespace ECommerce.Application.Services.Admin
{
    public class PermissionGroupService : IPermissionGroupService
    {
        private readonly IPermissionGroupRepository _repo;

        public PermissionGroupService(IPermissionGroupRepository repo) => _repo = repo;

        public async Task<PagedResult<PermissionGroupDto>> GetPagedAsync(int page, int pageSize, string? search = null, string? sortBy = null)
        {
            var paged = await _repo.GetPagedAsync(page, pageSize, search, sortBy);
            return new PagedResult<PermissionGroupDto>
            {
                Items = paged.Items.Select(g => new PermissionGroupDto
                {
                    Id = g.Id,
                    Name = g.Name,
                    Description = g.Description
                }),
                TotalCount = paged.TotalCount,
                Page = paged.Page,
                PageSize = paged.PageSize
            };
        }

        public async Task<PermissionGroupDto> CreateAsync(string name, string? description, IEnumerable<Guid>? permissionIds = null)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name is required.");

            if (await _repo.ExistsByNameAsync(name))
                throw new InvalidOperationException("A permission group with this name already exists.");

            var group = new PermissionGroup
            {
                Name = name.Trim(),
                Description = description?.Trim()
            };
            await _repo.AddAsync(group);
            await _repo.SaveChangesAsync();
            if (permissionIds != null)
            {
                await SyncGroupPermissions(group.Id, permissionIds);
                await _repo.SaveChangesAsync();   // ← ADD THIS LINE
            }

            return new PermissionGroupDto { Id = group.Id, Name = group.Name, Description = group.Description };
        }

        public async Task<PermissionGroupDto?> UpdateAsync(Guid id, string name, string? description, IEnumerable<Guid>? permissionIds = null)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name is required.");

            var group = await _repo.GetByIdAsync(id);
            if (group == null) return null;

            if (await _repo.ExistsByNameAsync(name, excludeId: id))
                throw new InvalidOperationException("A permission group with this name already exists.");

            group.Name = name.Trim();
            group.Description = description?.Trim();
            _repo.Update(group);
            await _repo.SaveChangesAsync();

            if (permissionIds != null)
            {
                await SyncGroupPermissions(group.Id, permissionIds);
                await _repo.SaveChangesAsync();   // ← ADD THIS LINE
            }
            return new PermissionGroupDto { Id = group.Id, Name = group.Name, Description = group.Description };
        }

        public async Task DeleteAsync(Guid id)
        {
            var group = await _repo.GetByIdAsync(id)
                        ?? throw new InvalidOperationException("Permission group not found.");

            if (await _repo.HasPermissionsAsync(id))
                throw new InvalidOperationException("Cannot delete permission group. Remove all permissions from this group first.");

            _repo.Delete(group);
            await _repo.SaveChangesAsync();
        }
        private async Task SyncGroupPermissions(Guid groupId, IEnumerable<Guid> permissionIds)
        {
            // 1. Remove all existing permissions for this group (already saves internally)
            await _repo.RemoveAllPermissionsFromGroupAsync(groupId);

            // 2. Add each new permission using the existing AddPermissionToGroupAsync
            foreach (var permId in permissionIds)
            {
                await _repo.AddPermissionToGroupAsync(groupId, permId);
            }

            // 3. Save all changes at once
            await _repo.SaveChangesAsync();
        }
    }
}