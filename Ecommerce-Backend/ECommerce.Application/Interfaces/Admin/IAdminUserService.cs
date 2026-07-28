using ECommerce.Application.DTOs.Admin;
using ECommerce.Application.Helpers;

namespace ECommerce.Application.Interfaces
{
    public interface IAdminUserService
    {
        Task<PagedResult<UserAdminDto>> GetUsersAsync(int page, int pageSize, string? search = null, string? role = null, bool? isActive = null);
        Task<UserAdminDto?> GetUserByIdAsync(Guid userId);
        Task ActivateUserAsync(Guid userId);
        Task DeactivateUserAsync(Guid userId);
    }
}