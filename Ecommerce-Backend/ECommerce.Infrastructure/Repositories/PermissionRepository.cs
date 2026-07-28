using ECommerce.Application.Interfaces;
using ECommerce.Domain.Entities;
using ECommerce.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Infrastructure.Repositories
{
    public class PermissionRepository : IPermissionRepository
    {
        private readonly ECommerceDbContext _context;
        public PermissionRepository(ECommerceDbContext context) => _context = context;

        public async Task<IEnumerable<Permission>> GetAllAsync()
            => await _context.Permissions.OrderBy(p => p.Code).ToListAsync();

        public async Task<Permission?> GetByIdAsync(Guid id)
            => await _context.Permissions.FindAsync(id);

        public async Task<Permission?> GetByCodeAsync(string code)
            => await _context.Permissions.FirstOrDefaultAsync(p => p.Code == code);

        public async Task<bool> ExistsByCodeAsync(string code, Guid? excludeId = null)
        {
            var query = _context.Permissions.Where(p => p.Code == code);
            if (excludeId.HasValue) query = query.Where(p => p.Id != excludeId.Value);
            return await query.AnyAsync();
        }

        public async Task AddAsync(Permission permission) => await _context.Permissions.AddAsync(permission);
        public void Update(Permission permission) => _context.Permissions.Update(permission);
        public void Delete(Permission permission) => _context.Permissions.Remove(permission);
        public async Task SaveChangesAsync() => await _context.SaveChangesAsync();
    }
}