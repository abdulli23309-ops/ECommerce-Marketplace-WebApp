using ECommerce.Application.DTOs.Admin;

namespace ECommerce.Application.Interfaces
{
    public interface IPermissionService
    {
        Task<IEnumerable<PermissionDto>> GetAllAsync();
        Task<PermissionDto> CreateAsync(string name, string code, string? description);
        Task<PermissionDto?> UpdateAsync(Guid id, string name, string code, string? description);
        Task DeleteAsync(Guid id);
    }
}