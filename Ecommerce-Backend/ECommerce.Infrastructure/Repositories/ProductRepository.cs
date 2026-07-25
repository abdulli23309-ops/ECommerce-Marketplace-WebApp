using ECommerce.Application.Helpers;
using ECommerce.Application.Interfaces;
using ECommerce.Domain.Entities;
using ECommerce.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Infrastructure.Repositories.Catalog
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

        // Bug fix: public listing must only return Approved products
        public async Task<PagedResult<Product>> GetPagedAsync(int page, int pageSize,
            Guid? categoryId = null, Guid? subCategoryId = null, Guid? brandId = null,
            decimal? minPrice = null, decimal? maxPrice = null,
            string? search = null, string? sortBy = null)
        {
            var query = _context.Products
                .Where(p => !p.IsDeleted && p.Status == "Approved")   // <-- added status filter
                .AsQueryable();

            // Optional filters (will be expanded in Task 9; for now only status is applied)
            // The method signature supports extra parameters for future Task 9.

            return await query.ToPagedResultAsync(page, pageSize);
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
            => await _context.Products
                .Include(p => p.Store)
                .ToListAsync();

        public async Task<Product?> GetByIdWithDetailsAsync(Guid productId)
            => await _context.Products
                .Include(p => p.ProductImages)
                .Include(p => p.Store)
                .Include(p => p.Brand)
                .Include(p => p.SubCategory)
                    .ThenInclude(sc => sc!.Category)
                .FirstOrDefaultAsync(p => p.Id == productId && !p.IsDeleted);
        public async Task<PagedResult<Product>> GetPagedAsync(int page, int pageSize)
    => await GetPagedAsync(page, pageSize, null, null, null, null, null, null, null);
        public async Task<int> GetProductCountAsync()
    => await _context.Products.CountAsync(p => !p.IsDeleted);

        public async Task<int> GetPendingProductCountAsync()
            => await _context.Products.CountAsync(p => p.Status == "PendingApproval");

        public async Task SaveChangesAsync() => await _context.SaveChangesAsync();
    }
}