using ECommerce.Application.DTOs.Product;
using ECommerce.Application.Helpers;

namespace ECommerce.Application.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDto>> GetStoreProductsAsync(Guid userId);
        Task<ProductDto> CreateProductAsync(Guid userId, CreateProductDto dto);
        Task<PagedResult<ProductDto>> GetPagedProductsAsync(int page, int pageSize);
        Task<ProductDto?> UpdateProductAsync(Guid productId, UpdateProductDto dto);
    }
}