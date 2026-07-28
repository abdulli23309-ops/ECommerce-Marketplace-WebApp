using ECommerce.Application.Interfaces;
using ECommerce.Domain.Entities;
using ECommerce.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Infrastructure.Repositories
{
    public class RolePermissionGroupRepository : IRolePermissionGroupRepository
    {
        private readonly ECommerceDbContext _context;
        public RolePermissionGroupRepository(ECommerceDbContext context) => _context = context;

        public async Task<bool> ExistsAsync(Guid roleId, Guid groupId)
            => await _context.RolePermissionGroups
                .AnyAsync(rpg => rpg.RoleId == roleId && rpg.PermissionGroupId == groupId);

        public async Task AddAsync(RolePermissionGroup link)
        {
            if (!await ExistsAsync(link.RoleId, link.PermissionGroupId))
                _context.RolePermissionGroups.Add(link);
        }

        public async Task RemoveAsync(Guid roleId, Guid groupId)
        {
            var link = await _context.RolePermissionGroups
                .FirstOrDefaultAsync(rpg => rpg.RoleId == roleId && rpg.PermissionGroupId == groupId);
            if (link != null) _context.RolePermissionGroups.Remove(link);
        }

        public async Task<IEnumerable<Guid>> GetGroupIdsByRoleIdAsync(Guid roleId)
            => await _context.RolePermissionGroups
                .Where(rpg => rpg.RoleId == roleId)
                .Select(rpg => rpg.PermissionGroupId)
                .ToListAsync();

        public async Task SaveChangesAsync() => await _context.SaveChangesAsync();
    }
}