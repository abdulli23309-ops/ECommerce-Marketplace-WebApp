using ECommerce.Application.Interfaces;
using ECommerce.Domain.Entities;
using ECommerce.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Infrastructure.Repositories
{
    public class AddressRepository : IAddressRepository
    {
        private readonly ECommerceDbContext _context;
        public AddressRepository(ECommerceDbContext context) => _context = context;

        public async Task<IEnumerable<Address>> GetByUserIdAsync(Guid userId)
            => await _context.Addresses.Where(a => a.UserId == userId).ToListAsync();

        public async Task<Address?> GetByIdAsync(Guid addressId)
            => await _context.Addresses.FindAsync(addressId);

        public async Task AddAsync(Address address)
            => await _context.Addresses.AddAsync(address);

        public void Update(Address address)
            => _context.Addresses.Update(address);

        public void Delete(Address address)
            => _context.Addresses.Remove(address);

        public async Task SaveChangesAsync() => await _context.SaveChangesAsync();
    }
}