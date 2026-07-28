using ECommerce.Application.Helpers;
using ECommerce.Domain.Entities;

namespace ECommerce.Application.Interfaces
{
    public interface IPermissionRepository
    {
        Task<IEnumerable<Permission>> GetAllAsync();
        Task<Permission?> GetByIdAsync(Guid id);
        Task<Permission?> GetByCodeAsync(string code);
        Task<bool> ExistsByCodeAsync(string code, Guid? excludeId = null);
        Task AddAsync(Permission permission);
        void Update(Permission permission);
        void Delete(Permission permission);
        Task SaveChangesAsync();
    }
}