using ECommerce.Application.Helpers;
using ECommerce.Application.DTOs.Admin;

namespace ECommerce.Application.Interfaces
{
    public interface IPermissionGroupService
    {
        Task<PagedResult<PermissionGroupDto>> GetPagedAsync(int page, int pageSize, string? search = null, string? sortBy = null);
        
        Task<PermissionGroupDto> CreateAsync(string name, string? description, IEnumerable<Guid>? permissionIds = null);
        Task<PermissionGroupDto?> UpdateAsync(Guid id, string name, string? description, IEnumerable<Guid>? permissionIds = null);
        Task DeleteAsync(Guid id);
    }
}