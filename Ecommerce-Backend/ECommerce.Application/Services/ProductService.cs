using ECommerce.Application.DTOs.Product;
using ECommerce.Application.Helpers;
using ECommerce.Application.Interfaces;
using ECommerce.Domain.Entities;

namespace ECommerce.Application.Services.Catalog
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepo;
        private readonly ISellerRepository _sellerRepo;
        private readonly IReviewRepository _reviewRepo;   // added for rating data

        public ProductService(
            IProductRepository productRepo,
            ISellerRepository sellerRepo,
            IReviewRepository reviewRepo)
        {
            _productRepo = productRepo;
            _sellerRepo = sellerRepo;
            _reviewRepo = reviewRepo;
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

        public async Task<ProductDto?> UpdateProductAsync(Guid userId, Guid productId, UpdateProductDto dto)
        {
            // Ownership check (already in Phase 0)
            var product = await _productRepo.GetByIdAsync(productId);
            if (product == null) return null;

            var profile = await _sellerRepo.GetByUserIdAsync(userId);
            if (profile == null) throw new UnauthorizedAccessException("Not a seller.");

            var store = await _sellerRepo.GetStoreBySellerIdAsync(profile.Id);
            if (store == null || product.StoreId != store.Id)
                throw new UnauthorizedAccessException("You do not own this product.");

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

        public async Task<bool> DeleteProductAsync(Guid userId, Guid productId)
        {
            var product = await _productRepo.GetByIdAsync(productId);
            if (product == null) return false;

            var profile = await _sellerRepo.GetByUserIdAsync(userId);
            if (profile == null) throw new UnauthorizedAccessException("Not a seller.");

            var store = await _sellerRepo.GetStoreBySellerIdAsync(profile.Id);
            if (store == null || product.StoreId != store.Id)
                throw new UnauthorizedAccessException("You do not own this product.");

            product.IsDeleted = true;
            product.UpdatedAt = DateTime.UtcNow;
            _productRepo.Update(product);
            await _productRepo.SaveChangesAsync();
            return true;
        }

        public async Task<ProductDetailDto?> GetProductDetailAsync(Guid productId)
        {
            var product = await _productRepo.GetByIdWithDetailsAsync(productId);
            if (product == null) return null;

            // Compute rating
            var reviews = await _reviewRepo.GetByProductIdAsync(productId);
            double? avgRating = null;
            int reviewCount = 0;
            if (reviews.Any())
            {
                reviewCount = reviews.Count();
                avgRating = reviews.Average(r => r.Rating);
            }

            return new ProductDetailDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                BasePrice = product.BasePrice,
                StockQuantity = product.StockQuantity,
                Status = product.Status,
                Images = product.ProductImages
                    .OrderBy(i => i.SortOrder)
                    .Select(i => new ProductImageDto
                    {
                        Id = i.Id,
                        ImageUrl = i.ImageUrl,
                        SortOrder = i.SortOrder
                    }).ToList(),
                BrandId = product.BrandId,
                BrandName = product.Brand?.Name,
                SubCategoryId = product.SubCategoryId,
                SubCategoryName = product.SubCategory?.Name,
                CategoryId = product.SubCategory?.CategoryId,
                CategoryName = product.SubCategory?.Category?.Name,
                StoreId = product.StoreId,
                StoreName = product.Store?.Name ?? "",
                AverageRating = avgRating,
                ReviewCount = reviewCount,
                CreatedAt = product.CreatedAt
            };
        }
        public async Task<IEnumerable<ProductDto>> GetPublicStoreProductsAsync(Guid storeId)
        {
            // Use the repository to get products by StoreId, filtered to approved and not deleted
            var products = await _productRepo.GetByStoreIdAsync(storeId);  // This already exists, but it filters by StoreId + !IsDeleted. We need to also filter by Approved status.
            return products
                .Where(p => p.Status == "Approved")
                .Select(p => new ProductDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    BasePrice = p.BasePrice,
                    Status = p.Status,
                    StockQuantity = p.StockQuantity,
                    StoreId = p.StoreId,
                    Images = p.ProductImages.Select(i => i.ImageUrl).ToList()
                });
        }
    }
}