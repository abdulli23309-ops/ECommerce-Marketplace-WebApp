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
        private readonly IFileStorageService _fileStorageService;
        private readonly IReviewRepository _reviewRepo;   // added for rating data

        public ProductService(
            IProductRepository productRepo,
            ISellerRepository sellerRepo,
            IReviewRepository reviewRepo, IFileStorageService fileStorageService)
        {
            _productRepo = productRepo;
            _sellerRepo = sellerRepo;
            _reviewRepo = reviewRepo;
            _fileStorageService = fileStorageService;
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
                StockQuantity = p.StockQuantity,   
                StoreId = p.StoreId,
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
                StockQuantity = dto.StockQuantity,       // <-- add
                SubCategoryId = dto.SubCategoryId,
                BrandId = dto.BrandId,                   // <-- add
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
                Status = product.Status,
                StockQuantity = product.StockQuantity,   // include in return DTO
                StoreId = product.StoreId,
                Images = product.ProductImages.Select(i => i.ImageUrl).ToList()
            };
        }

        public async Task<PagedResult<ProductDto>> GetPagedProductsAsync(
    int page, int pageSize,
    Guid? categoryId = null, Guid? subCategoryId = null, Guid? brandId = null,
    decimal? minPrice = null, decimal? maxPrice = null,
    string? search = null, string? sortBy = null)
        {
            var paged = await _productRepo.GetPagedAsync(page, pageSize,
                categoryId, subCategoryId, brandId, minPrice, maxPrice, search, sortBy);

            return new PagedResult<ProductDto>
            {
                Items = paged.Items.Select(p => new ProductDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    BasePrice = p.BasePrice,
                    Status = p.Status,
                    StockQuantity = p.StockQuantity,
                    StoreId = p.StoreId,
                    Images = p.ProductImages.Select(i => i.ImageUrl).ToList()
                }),
                TotalCount = paged.TotalCount,
                Page = paged.Page,
                PageSize = paged.PageSize
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
                StockQuantity = product.StockQuantity,   // ← add this
                StoreId = product.StoreId,
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
                StoreLogoUrl = product.Store?.LogoUrl,
                StoreDescription = product.Store?.Description,
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
        public async Task<ProductImageDto> UploadProductImageAsync(Guid userId, Guid productId, Stream fileStream, string fileName)
        {
            var product = await _productRepo.GetByIdAsync(productId)
                          ?? throw new InvalidOperationException("Product not found.");

            // Ownership check – same as UpdateProductAsync
            var profile = await _sellerRepo.GetByUserIdAsync(userId)
                          ?? throw new UnauthorizedAccessException("Not a seller.");
            var store = await _sellerRepo.GetStoreBySellerIdAsync(profile.Id)
                        ?? throw new UnauthorizedAccessException("No store.");
            if (product.StoreId != store.Id)
                throw new UnauthorizedAccessException("You do not own this product.");

            // Save file using the storage service
            var imageUrl = await _fileStorageService.SaveFileAsync(fileStream, fileName, "products");

            var productImage = new ProductImage
            {
                ProductId = productId,
                ImageUrl = imageUrl,
                SortOrder = 0, // could be set to last order, but simple default fine
                CreatedAt = DateTime.UtcNow
            };
            await _productRepo.AddImageAsync(productImage);
            await _productRepo.SaveChangesAsync();

            return new ProductImageDto
            {
                Id = productImage.Id,
                ImageUrl = productImage.ImageUrl,
                SortOrder = productImage.SortOrder
            };
        }

        public async Task DeleteProductImageAsync(Guid userId, Guid productId, Guid imageId)
        {
            var product = await _productRepo.GetByIdAsync(productId)
                          ?? throw new InvalidOperationException("Product not found.");

            // Ownership check
            var profile = await _sellerRepo.GetByUserIdAsync(userId)
                          ?? throw new UnauthorizedAccessException("Not a seller.");
            var store = await _sellerRepo.GetStoreBySellerIdAsync(profile.Id)
                        ?? throw new UnauthorizedAccessException("No store.");
            if (product.StoreId != store.Id)
                throw new UnauthorizedAccessException("You do not own this product.");

            var image = await _productRepo.GetImageByIdAsync(imageId)
                        ?? throw new InvalidOperationException("Image not found.");
            if (image.ProductId != productId)
                throw new InvalidOperationException("Image does not belong to the specified product.");

            // Delete physical file
            _fileStorageService.DeleteFile(image.ImageUrl);

            _productRepo.DeleteImage(image);
            await _productRepo.SaveChangesAsync();
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