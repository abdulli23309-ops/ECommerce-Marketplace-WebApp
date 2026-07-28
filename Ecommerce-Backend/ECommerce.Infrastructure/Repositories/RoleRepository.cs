using ECommerce.Application.Interfaces;
using ECommerce.Application.Interfaces.Admin;
using ECommerce.Domain.Entities;
using ECommerce.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Infrastructure.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly ECommerceDbContext _context;
        public RoleRepository(ECommerceDbContext context) => _context = context;

        public async Task<IEnumerable<Role>> GetAllAsync()
            => await _context.Roles.OrderBy(r => r.Name).ToListAsync();
        public async Task<Role?> GetByIdAsync(Guid id) => await _context.Roles.FindAsync(id);
        public async Task<IEnumerable<Role>> GetRolesWithPermissionGroupsAsync()
    => await _context.Roles
        .Where(r => r.RolePermissionGroups.Any())
        .OrderBy(r => r.Name)
        .ToListAsync();
    }
}