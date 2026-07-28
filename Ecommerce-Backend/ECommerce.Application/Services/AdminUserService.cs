using ECommerce.Application.DTOs.Admin;
using ECommerce.Application.Helpers;
using ECommerce.Application.Interfaces;

namespace ECommerce.Application.Services.Admin
{
    public class AdminUserService : IAdminUserService
    {
        private readonly IUserRepository _userRepo;

        public AdminUserService(IUserRepository userRepo) => _userRepo = userRepo;

        public async Task<PagedResult<UserAdminDto>> GetUsersAsync(int page, int pageSize, string? search = null, string? role = null, bool? isActive = null)
        {
            var paged = await _userRepo.GetPagedAsync(page, pageSize, search, role, isActive);
            return new PagedResult<UserAdminDto>
            {
                Items = paged.Items.Select(u => new UserAdminDto
                {
                    Id = u.Id,
                    FullName = u.FullName,
                    Email = u.Email,
                    IsActive = u.IsActive,
                    CreatedAt = u.CreatedAt,
                    Roles = u.UserRoles.Select(ur => ur.Role.Name).ToList()
                }),
                TotalCount = paged.TotalCount,
                Page = paged.Page,
                PageSize = paged.PageSize
            };
        }

        public async Task<UserAdminDto?> GetUserByIdAsync(Guid userId)
        {
            var user = await _userRepo.GetByIdWithRolesAsync(userId);
            if (user == null) return null;
            return new UserAdminDto
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                IsActive = user.IsActive,
                CreatedAt = user.CreatedAt,
                Roles = user.UserRoles.Select(ur => ur.Role.Name).ToList()
            };
        }

        public async Task ActivateUserAsync(Guid userId) => await _userRepo.ActivateAsync(userId);
        public async Task DeactivateUserAsync(Guid userId) => await _userRepo.DeactivateAsync(userId);
    }
}