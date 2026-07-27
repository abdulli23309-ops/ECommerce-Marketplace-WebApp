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

        public async Task AddAsync(Product product) => await _context.Products.AddAsync(product);
        public void Update(Product product) => _context.Products.Update(product);
        public async Task AddImageAsync(ProductImage image) => await _context.ProductImages.AddAsync(image);

        public async Task<IEnumerable<Product>> GetAllAsync()
            => await _context.Products.Include(p => p.Store).ToListAsync();

        public async Task<PagedResult<Product>> GetPagedAsync(int page, int pageSize,
            Guid? categoryId, Guid? subCategoryId, Guid? brandId,
            decimal? minPrice, decimal? maxPrice, string? search, string? sortBy)
        {
            var query = _context.Products
                .Where(p => !p.IsDeleted && p.Status == "Approved")
                .Include(p => p.ProductImages)
                .AsQueryable();

            if (categoryId.HasValue)
                query = query.Where(p => p.SubCategory != null && p.SubCategory.CategoryId == categoryId.Value);

            if (subCategoryId.HasValue)
                query = query.Where(p => p.SubCategoryId == subCategoryId.Value);

            if (brandId.HasValue)
                query = query.Where(p => p.BrandId == brandId.Value);

            if (minPrice.HasValue)
                query = query.Where(p => p.BasePrice >= minPrice.Value);
            if (maxPrice.HasValue)
                query = query.Where(p => p.BasePrice <= maxPrice.Value);

            if (!string.IsNullOrWhiteSpace(search))
            {
                string term = search.Trim().ToLower();
                query = query.Where(p => p.Name.ToLower().Contains(term)
                                       || (p.Description != null && p.Description.ToLower().Contains(term)));
            }

            query = sortBy switch
            {
                "price_asc" => query.OrderBy(p => p.BasePrice),
                "price_desc" => query.OrderByDescending(p => p.BasePrice),
                _ => query.OrderByDescending(p => p.CreatedAt)
            };

            return await query.ToPagedResultAsync(page, pageSize);
        }

        public async Task<Product?> GetByIdWithDetailsAsync(Guid productId)
            => await _context.Products
                .Include(p => p.ProductImages)
                .Include(p => p.Store)
                .Include(p => p.Brand)
                .Include(p => p.SubCategory)
                    .ThenInclude(sc => sc!.Category)
                .FirstOrDefaultAsync(p => p.Id == productId && !p.IsDeleted);

        public async Task<int> GetProductCountAsync()
            => await _context.Products.CountAsync(p => !p.IsDeleted);

        public async Task<int> GetPendingProductCountAsync()
            => await _context.Products.CountAsync(p => p.Status == "PendingApproval");
        public async Task<ProductImage?> GetImageByIdAsync(Guid imageId)
    => await _context.ProductImages
        .Include(pi => pi.Product)   // needed for ownership check
        .FirstOrDefaultAsync(pi => pi.Id == imageId);

        public void DeleteImage(ProductImage image)
            => _context.ProductImages.Remove(image);

        public async Task SaveChangesAsync() => await _context.SaveChangesAsync();
    }
}