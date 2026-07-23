using ECommerce.Domain.Entities;

namespace ECommerce.Application.Interfaces
{
    public interface IBrandRepository
    {
        Task<IEnumerable<Brand>> GetAllAsync();
        Task<Brand?> GetByIdAsync(Guid id);
        Task AddAsync(Brand brand);
        void Update(Brand brand);
        void Delete(Brand brand);
        Task SaveChangesAsync();
    }
}