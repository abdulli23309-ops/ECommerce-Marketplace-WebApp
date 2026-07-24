using ECommerce.Application.DTOs.Product;
using ECommerce.Application.Helpers;
using ECommerce.Application.Interfaces;
using ECommerce.Domain.Entities;

namespace ECommerce.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepo;
        private readonly ISellerRepository _sellerRepo;

        public ProductService(IProductRepository productRepo, ISellerRepository sellerRepo)
        {
            _productRepo = productRepo;
            _sellerRepo = sellerRepo;
        }

        public async Task<IEnumerable<ProductDto>> GetStoreProductsAsync(Guid userId)
        {
            var profile = await _sellerRepo.GetByUserIdAsync(userId);
            if (profile == null) return new List<ProductDto>();

            var store = await _sellerRepo.GetStoreBySellerIdAsync(profile.Id);
            if (store == null) return new List<ProductDto>();

            var products = await _productRepo.GetByStoreIdAsync(store.Id);
            return products.Select(p => new ProductDto
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                BasePrice = p.BasePrice,
                Status = p.Status,
                Images = p.ProductImages.Select(i => i.ImageUrl).ToList()
            });
        }

        public async Task<ProductDto> CreateProductAsync(Guid userId, CreateProductDto dto)
        {
            var profile = await _sellerRepo.GetByUserIdAsync(userId)
                          ?? throw new InvalidOperationException("Seller profile required.");
            var store = await _sellerRepo.GetStoreBySellerIdAsync(profile.Id)
                        ?? throw new InvalidOperationException("Store required.");

            var product = new Product
            {
                StoreId = store.Id,
                Name = dto.Name,
                Description = dto.Description,
                BasePrice = dto.BasePrice,
                SubCategoryId = dto.SubCategoryId,
                Status = "PendingApproval",
                CreatedAt = DateTime.UtcNow
            };

            await _productRepo.AddAsync(product);
            await _productRepo.SaveChangesAsync();

            return new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                BasePrice = product.BasePrice,
                Status = product.Status
            };
        }
        public async Task<PagedResult<ProductDto>> GetPagedProductsAsync(int page, int pageSize)
        {
            var pagedProducts = await _productRepo.GetPagedAsync(page, pageSize);

            return new PagedResult<ProductDto>
            {
                Items = pagedProducts.Items.Select(p => new ProductDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    BasePrice = p.BasePrice,
                    Status = p.Status,
                    Images = p.ProductImages.Select(i => i.ImageUrl).ToList()
                }),
                TotalCount = pagedProducts.TotalCount,
                Page = pagedProducts.Page,
                PageSize = pagedProducts.PageSize
            };
        }
        public async Task<ProductDto?> UpdateProductAsync(Guid productId, UpdateProductDto dto)
        {
            var product = await _productRepo.GetByIdAsync(productId);
            if (product == null) return null;

            product.Name = dto.Name;
            product.Description = dto.Description;
            product.BasePrice = dto.BasePrice;
            product.StockQuantity = dto.StockQuantity;
            product.SubCategoryId = dto.SubCategoryId;
            product.BrandId = dto.BrandId;
            product.UpdatedAt = DateTime.UtcNow;

            _productRepo.Update(product);
            await _productRepo.SaveChangesAsync();

            return new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                BasePrice = product.BasePrice,
                Status = product.Status,
                Images = product.ProductImages.Select(i => i.ImageUrl).ToList()
            };
        }
    }
}