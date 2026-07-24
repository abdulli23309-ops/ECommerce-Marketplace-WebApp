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
        Task<PagedResult<Product>> GetPagedAsync(int page, int pageSize);
        
        Task SaveChangesAsync();
    }
}