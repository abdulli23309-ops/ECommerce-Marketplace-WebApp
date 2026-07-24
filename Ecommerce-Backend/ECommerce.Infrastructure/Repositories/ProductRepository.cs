using ECommerce.Application.Helpers;
using ECommerce.Application.Interfaces;
using ECommerce.Domain.Entities;
using ECommerce.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ECommerceDbContext _context;
        public ProductRepository(ECommerceDbContext context) => _context = context;

        public async Task<IEnumerable<Product>> GetByStoreIdAsync(Guid storeId)
            => await _context.Products
                .Where(p => p.StoreId == storeId && !p.IsDeleted)
                .Include(p => p.ProductImages)
                .ToListAsync();

        public async Task<Product?> GetByIdAsync(Guid productId)
            => await _context.Products
                .Include(p => p.ProductImages)
                .FirstOrDefaultAsync(p => p.Id == productId && !p.IsDeleted);

        public async Task AddAsync(Product product)
            => await _context.Products.AddAsync(product);

        public void Update(Product product)
            => _context.Products.Update(product);

        public async Task AddImageAsync(ProductImage image)
            => await _context.ProductImages.AddAsync(image);

        public async Task<PagedResult<Product>> GetPagedAsync(int page, int pageSize)
        {
            var query = _context.Products
                .Where(p => !p.IsDeleted)
                .AsQueryable();

            return await query.ToPagedResultAsync(page, pageSize);
        }
        public async Task<IEnumerable<Product>> GetAllAsync()
    => await _context.Products
        .Include(p => p.Store)
        .ToListAsync();

        public async Task SaveChangesAsync() => await _context.SaveChangesAsync();
    }
}