using ECommerce.Application.Helpers;
using ECommerce.Application.Interfaces;
using ECommerce.Domain.Entities;
using ECommerce.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ECommerceDbContext _context;

        public UserRepository(ECommerceDbContext context)
        {
            _context = context;
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User?> GetByIdAsync(Guid id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task AddAsync(User user)
        {
            await _context.Users.AddAsync(user);
        }

        public void Update(User user)
        {
            _context.Users.Update(user);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
        public async Task<User?> GetByRefreshTokenAsync(string refreshToken)
        {
            return await _context.Users
                .Include(u => u.RefreshTokens)
                .FirstOrDefaultAsync(u => u.RefreshTokens.Any(rt => rt.Token == refreshToken));
        }
        public async Task<int> GetUserCountAsync()
    => await _context.Users.CountAsync();
        public async Task<IList<string>> GetUserRolesAsync(Guid userId)
        {
            return await _context.UserRoles
                .Where(ur => ur.UserId == userId)
                .Select(ur => ur.Role.Name)
                .ToListAsync();
        }
        public async Task<PagedResult<User>> GetPagedAsync(int page, int pageSize, string? search = null, string? role = null, bool? isActive = null)
        {
            var query = _context.Users
                .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                var term = search.Trim().ToLower();
                query = query.Where(u => u.FullName.ToLower().Contains(term)
                                         || u.Email.ToLower().Contains(term));
            }

            if (!string.IsNullOrWhiteSpace(role))
                query = query.Where(u => u.UserRoles.Any(ur => ur.Role.Name == role));

            if (isActive.HasValue)
                query = query.Where(u => u.IsActive == isActive.Value);

            return await query.OrderByDescending(u => u.CreatedAt).ToPagedResultAsync(page, pageSize);
        }

        public async Task<User?> GetByIdWithRolesAsync(Guid userId)
            => await _context.Users
                .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
                .FirstOrDefaultAsync(u => u.Id == userId);

        public async Task ActivateAsync(Guid userId)
        {
            var user = await _context.Users.FindAsync(userId)
                       ?? throw new InvalidOperationException("User not found.");
            user.IsActive = true;
            user.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
        }

        public async Task DeactivateAsync(Guid userId)
        {
            var user = await _context.Users.FindAsync(userId)
                       ?? throw new InvalidOperationException("User not found.");
            user.IsActive = false;
            user.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
        }
        public async Task<List<string>> GetPermissionCodesAsync(Guid userId)
        {
            return await _context.UserRoles
                .Where(ur => ur.UserId == userId)
                .SelectMany(ur => ur.Role.RolePermissionGroups)
                .SelectMany(rpg => rpg.PermissionGroup.PermissionGroupPermissions)
                .Select(pgp => pgp.Permission.Code)
                .Distinct()
                .ToListAsync();
        }
        public async Task AddUserRoleAsync(Guid userId, string roleName)
        {
            var role = await _context.Roles.FirstOrDefaultAsync(r => r.Name == roleName)
                       ?? throw new InvalidOperationException($"Role '{roleName}' not found.");
            if (!await _context.UserRoles.AnyAsync(ur => ur.UserId == userId && ur.RoleId == role.Id))
            {
                _context.UserRoles.Add(new UserRole { UserId = userId, RoleId = role.Id });
            }
        }
    }
}