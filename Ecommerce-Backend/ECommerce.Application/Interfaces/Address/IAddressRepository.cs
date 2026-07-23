using ECommerce.Domain.Entities;

namespace ECommerce.Application.Interfaces
{
    public interface IAddressRepository
    {
        Task<IEnumerable<Address>> GetByUserIdAsync(Guid userId);
        Task<Address?> GetByIdAsync(Guid addressId);
        Task AddAsync(Address address);
        void Update(Address address);
        void Delete(Address address);
        Task SaveChangesAsync();
    }
}