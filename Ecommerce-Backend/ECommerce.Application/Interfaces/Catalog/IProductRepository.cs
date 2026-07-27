using ECommerce.Application.Helpers;
using ECommerce.Domain.Entities;

namespace ECommerce.Application.Interfaces
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetByStoreIdAsync(Guid storeId);
        Task<Product?> GetByIdAsync(Guid productId);
        Task AddAsync(Product product);
        void Update(Product product);
        Task AddImageAsync(ProductImage image);
        Task<IEnumerable<Product>> GetAllAsync();
        Task<PagedResult<Product>> GetPagedAsync(int page, int pageSize,
    Guid? categoryId = null, Guid? subCategoryId = null, Guid? brandId = null,
    decimal? minPrice = null, decimal? maxPrice = null,
    string? search = null, string? sortBy = null);
        Task<Product?> GetByIdWithDetailsAsync(Guid productId);
        Task<int> GetProductCountAsync();
        Task<int> GetPendingProductCountAsync();
        Task<ProductImage?> GetImageByIdAsync(Guid imageId);
        void DeleteImage(ProductImage image);

        Task SaveChangesAsync();
    }
}