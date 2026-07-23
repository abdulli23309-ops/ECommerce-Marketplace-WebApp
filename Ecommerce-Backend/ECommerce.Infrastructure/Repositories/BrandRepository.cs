using ECommerce.Application.Interfaces;
using ECommerce.Domain.Entities;
using ECommerce.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Infrastructure.Repositories.Catalog
{
    public class BrandRepository : IBrandRepository
    {
        private readonly ECommerceDbContext _context;
        public BrandRepository(ECommerceDbContext context) => _context = context;

        public async Task<IEnumerable<Brand>> GetAllAsync()
            => await _context.Brands.ToListAsync();

        public async Task<Brand?> GetByIdAsync(Guid id)
            => await _context.Brands.FirstOrDefaultAsync(b => b.Id == id);

        public async Task AddAsync(Brand brand)
            => await _context.Brands.AddAsync(brand);

        public void Update(Brand brand)
            => _context.Brands.Update(brand);

        public void Delete(Brand brand)
            => _context.Brands.Remove(brand);

        public async Task SaveChangesAsync() => await _context.SaveChangesAsync();
    }
}