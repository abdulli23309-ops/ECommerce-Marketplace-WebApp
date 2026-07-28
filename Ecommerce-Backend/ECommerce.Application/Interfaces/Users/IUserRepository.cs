using ECommerce.Application.Helpers;
using ECommerce.Domain.Entities;

namespace ECommerce.Application.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetByEmailAsync(string email);
        Task<User?> GetByIdAsync(Guid id);
        Task AddAsync(User user);
        void Update(User user);
        Task SaveChangesAsync();
        Task AddUserRoleAsync(Guid userId, string roleName);
        Task<User?> GetByRefreshTokenAsync(string refreshToken);
        Task<List<string>> GetPermissionCodesAsync(Guid userId);
        Task<PagedResult<User>> GetPagedAsync(int page, int pageSize, string? search = null, string? role = null, bool? isActive = null);
        Task<User?> GetByIdWithRolesAsync(Guid userId);
        Task ActivateAsync(Guid userId);
        Task DeactivateAsync(Guid userId);
        Task<int> GetUserCountAsync();
        Task<IList<string>> GetUserRolesAsync(Guid userId);
    }
}