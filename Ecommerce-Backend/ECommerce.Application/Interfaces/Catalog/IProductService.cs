using ECommerce.Application.DTOs.Product;

namespace ECommerce.Application.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDto>> GetStoreProductsAsync(Guid userId);
        Task<ProductDto> CreateProductAsync(Guid userId, CreateProductDto dto);
    }
}