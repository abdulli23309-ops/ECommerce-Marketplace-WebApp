using ECommerce.Application.Helpers;
using ECommerce.Application.Interfaces;
using ECommerce.Domain.Entities;
using ECommerce.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace ECommerce.Infrastructure.Repositories
{
    public class PermissionGroupRepository : IPermissionGroupRepository
    {
        private readonly ECommerceDbContext _context;
        public PermissionGroupRepository(ECommerceDbContext context) => _context = context;

        public async Task<PagedResult<PermissionGroup>> GetPagedAsync(int page, int pageSize, string? search = null, string? sortBy = null)
        {
            var query = _context.PermissionGroups.AsQueryable();
            if (!string.IsNullOrWhiteSpace(search))
            {
                string term = search.Trim().ToLower();
                query = query.Where(g => g.Name.ToLower().Contains(term)
                                    || (g.Description != null && g.Description.ToLower().Contains(term)));
            }
            query = sortBy switch
            {
                "name_asc" => query.OrderBy(g => g.Name),
                "name_desc" => query.OrderByDescending(g => g.Name),
                _ => query.OrderBy(g => g.Name)   // default
            };
            return await query.ToPagedResultAsync(page, pageSize);
        }

        public async Task<PermissionGroup?> GetByIdAsync(Guid id)
            => await _context.PermissionGroups.FindAsync(id);

        public async Task<bool> ExistsByNameAsync(string name, Guid? excludeId = null)
        {
            var query = _context.PermissionGroups.Where(g => g.Name.ToLower() == name.ToLower());
            if (excludeId.HasValue)
                query = query.Where(g => g.Id != excludeId.Value);
            return await query.AnyAsync();
        }

        public async Task<bool> HasPermissionsAsync(Guid groupId)
            =>await _context.PermissionGroupPermissions.AnyAsync(gp => gp.PermissionGroupId == groupId);

        public async Task AddAsync(PermissionGroup group)
            => await _context.PermissionGroups.AddAsync(group);

        public void Update(PermissionGroup group)
            => _context.PermissionGroups.Update(group);

        public void Delete(PermissionGroup group)
            => _context.PermissionGroups.Remove(group);
        public async Task AddPermissionToGroupAsync(Guid groupId, Guid permissionId)
        {
            if (!await _context.PermissionGroupPermissions.AnyAsync(gp => gp.PermissionGroupId == groupId && gp.PermissionId == permissionId))
            {
                _context.PermissionGroupPermissions.Add(new PermissionGroupPermission { PermissionGroupId = groupId, PermissionId = permissionId });
            }
        }
        public async Task RemoveAllPermissionsFromGroupAsync(Guid groupId)
        {
            var toRemove = await _context.PermissionGroupPermissions
                .Where(gp => gp.PermissionGroupId == groupId)
                .ToListAsync();
            _context.PermissionGroupPermissions.RemoveRange(toRemove);
            await _context.SaveChangesAsync();
        }

        public async Task ExecuteSqlRawAsync(string sql)
        {
            await _context.Database.ExecuteSqlRawAsync(sql);
        }
        public async Task RemovePermissionFromGroupAsync(Guid groupId, Guid permissionId)
        {
            var link = await _context.PermissionGroupPermissions
                .FirstOrDefaultAsync(gp => gp.PermissionGroupId == groupId && gp.PermissionId == permissionId);
            if (link != null) _context.PermissionGroupPermissions.Remove(link);
        }

        public async Task<IEnumerable<Guid>> GetPermissionIdsByGroupIdAsync(Guid groupId)
            => await _context.PermissionGroupPermissions
                .Where(gp => gp.PermissionGroupId == groupId)
                .Select(gp => gp.PermissionId)
                .ToListAsync();

        public async Task SaveChangesAsync() => await _context.SaveChangesAsync();
    }
}