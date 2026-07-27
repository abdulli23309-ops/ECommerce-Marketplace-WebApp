using ECommerce.Application.DTOs.Product;
using ECommerce.Application.Helpers;

namespace ECommerce.Application.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDto>> GetStoreProductsAsync(Guid userId);
        Task<ProductDto> CreateProductAsync(Guid userId, CreateProductDto dto);
        Task<PagedResult<ProductDto>> GetPagedProductsAsync(
    int page, int pageSize,
    Guid? categoryId = null, Guid? subCategoryId = null, Guid? brandId = null,
    decimal? minPrice = null, decimal? maxPrice = null,
    string? search = null, string? sortBy = null);
        // userId added for the Phase 0 security fix: the service now verifies the
        // calling seller actually owns the product's store before updating/deleting it.
        Task<ProductDto?> UpdateProductAsync(Guid userId, Guid productId, UpdateProductDto dto);
        Task<bool> DeleteProductAsync(Guid userId, Guid productId);
        Task<ProductDetailDto?> GetProductDetailAsync(Guid productId);
        Task<IEnumerable<ProductDto>> GetPublicStoreProductsAsync(Guid storeId);
        Task<ProductImageDto> UploadProductImageAsync(Guid userId, Guid productId, Stream fileStream, string fileName);
        Task DeleteProductImageAsync(Guid userId, Guid productId, Guid imageId);
    }
}