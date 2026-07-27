using ECommerce.Application.DTOs.Seller;
using ECommerce.Application.Helpers;
using ECommerce.Application.Interfaces;
using ECommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Application.Services.Seller
{
    public class SellerService : ISellerService
    {
        private readonly ISellerRepository _repo;
        private readonly IProductRepository _productRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IReviewRepository _reviewRepository;
        private readonly IShipmentRepository _shipmentRepository;
        public SellerService(ISellerRepository repo, IProductRepository productRepository, IOrderRepository orderRepository, IReviewRepository reviewRepository, IShipmentRepository shipmentRepository)
        {
            _repo = repo;
            _productRepository = productRepository;
            _orderRepository = orderRepository;
            _reviewRepository = reviewRepository;
            _shipmentRepository = shipmentRepository;
        }

        public async Task<SellerProfileDto?> GetProfileAsync(Guid userId)
        {
            var profile = await _repo.GetByUserIdAsync(userId);
            if (profile == null) return null;
            return new SellerProfileDto
            {
                Id = profile.Id,
                BusinessName = profile.BusinessName,
                Description = profile.Description,
                Status = profile.Status
            };
        }

        public async Task<SellerProfileDto> CreateProfileAsync(Guid userId, CreateSellerProfileDto dto)
        {
            var profile = new SellerProfile
            {
                UserId = userId,
                BusinessName = dto.BusinessName,
                Description = dto.Description,
                Status = "Pending",
                CreatedAt = DateTime.UtcNow
            };
            await _repo.AddProfileAsync(profile);
            await _repo.SaveChangesAsync();
            return new SellerProfileDto
            {
                Id = profile.Id,
                BusinessName = profile.BusinessName,
                Description = profile.Description,
                Status = profile.Status
            };
        }

        public async Task<StoreDto?> GetStoreAsync(Guid userId)
        {
            var profile = await _repo.GetByUserIdAsync(userId);
            if (profile == null) return null;
            var store = await _repo.GetStoreBySellerIdAsync(profile.Id);
            if (store == null) return null;
            return new StoreDto
            {
                Id = store.Id,
                Name = store.Name,
                Description = store.Description,
                IsActive = store.IsActive
            };
        }
        public async Task<SellerDashboardDto> GetDashboardAsync(Guid userId)
        {
            var profile = await _repo.GetByUserIdAsync(userId)
                          ?? throw new InvalidOperationException("Seller profile not found.");

            var store = await _repo.GetStoreBySellerIdAsync(profile.Id)
                        ?? throw new InvalidOperationException("Store not found.");

            var products = await _productRepository.GetByStoreIdAsync(store.Id);

            var approved = products.Where(p => p.Status == "Approved").Count();
            var pending = products.Where(p => p.Status == "PendingApproval").Count();
            var rejected = products.Where(p => p.Status == "Rejected" || p.Status == "Suspended").Count();

            var sellerOrders = await _orderRepository.GetSellerOrdersByStoreIdAsync(store.Id);
            var today = DateTime.UtcNow.Date;
            var firstOfMonth = new DateTime(today.Year, today.Month, 1);

            var todayOrders = sellerOrders.Count(o => o.CreatedAt.Date == today);
            var monthlyOrders = sellerOrders.Count(o => o.CreatedAt >= firstOfMonth);
            var revenue = sellerOrders.Sum(o => o.SubTotal); // assumes all seller orders are paid? simple

            var pendingShipments = await _shipmentRepository.GetPendingShipmentsCountByStoreIdAsync(store.Id);

            var averageRating = await _reviewRepository.GetAverageRatingByStoreIdAsync(store.Id);

            return new SellerDashboardDto
            {
                TotalProducts = products.Count(),
                ApprovedProducts = approved,
                PendingProducts = pending,
                RejectedProducts = rejected,
                TodayOrders = todayOrders,
                MonthlyOrders = monthlyOrders,
                TotalRevenue = revenue,
                PendingShipments = pendingShipments,
                AverageRating = averageRating
            };
        }
        public async Task<StoreDto?> UpdateStoreAsync(Guid userId, UpdateStoreDto dto)
        {
            var profile = await _repo.GetByUserIdAsync(userId);
            if (profile == null) return null;

            var store = await _repo.GetStoreBySellerIdAsync(profile.Id);
            if (store == null) return null;

            store.Name = dto.Name;
            store.Description = dto.Description;
            store.LogoUrl = dto.LogoUrl;
            store.UpdatedAt = DateTime.UtcNow;
            _repo.UpdateStore(store);
            await _repo.SaveChangesAsync();

            return new StoreDto
            {
                Id = store.Id,
                Name = store.Name,
                Description = store.Description,
                IsActive = store.IsActive
                // LogoUrl not in StoreDto? We need to add it or create a new DTO. We'll add LogoUrl to StoreDto.
            };
        }
        public async Task<StoreDto> CreateStoreAsync(Guid userId, CreateStoreDto dto)
        {
            var profile = await _repo.GetByUserIdAsync(userId);
            if (profile == null) throw new InvalidOperationException("Seller profile not found.");
            var store = new Store
            {
                SellerProfileId = profile.Id,
                Name = dto.Name,
                Description = dto.Description,
                LogoUrl = dto.LogoUrl,   // <-- add this
                CreatedAt = DateTime.UtcNow
            };
            await _repo.AddStoreAsync(store);
            await _repo.SaveChangesAsync();
            return new StoreDto
            {
                Id = store.Id,
                Name = store.Name,
                Description = store.Description,
                IsActive = store.IsActive,
                LogoUrl = store.LogoUrl   // include in return DTO
            };
        }
        public async Task<PagedResult<SellerReviewDto>> GetStoreReviewsAsync(Guid userId, int page, int pageSize)
        {
            var profile = await _repo.GetByUserIdAsync(userId)
                          ?? throw new InvalidOperationException("Seller profile not found.");
            var store = await _repo.GetStoreBySellerIdAsync(profile.Id)
                        ?? throw new InvalidOperationException("Store not found.");

            var reviews = await _reviewRepository.GetByStoreIdAsync(store.Id, page, pageSize);
            var totalCount = await GetStoreReviewCount(store.Id); // we'll add a helper

            var items = reviews.Select(r => new SellerReviewDto
            {
                ReviewId = r.Id,
                ProductId = r.ProductId ?? Guid.Empty,
                ProductName = r.Product?.Name ?? "Deleted Product",
                CustomerName = r.User?.FullName ?? "Anonymous",
                Rating = r.Rating,
                Comment = r.Comment,
                CreatedAt = r.CreatedAt,
                ImageUrls = r.ReviewImages.Select(i => i.ImageUrl).ToList()
            });

            return new PagedResult<SellerReviewDto>
            {
                Items = items,
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize
            };
        }

        private async Task<int> GetStoreReviewCount(Guid storeId)
        {
            return await _reviewRepository.GetCountByStoreIdAsync(storeId);
        }
        public async Task<StorePublicInfoDto?> GetStorePublicInfoAsync(Guid storeId)
        {
            var store = await _repo.GetStoreByIdAsync(storeId);
            if (store == null) return null;
            return new StorePublicInfoDto
            {
                StoreId = store.Id,
                Name = store.Name,
                Description = store.Description,
                LogoUrl = store.LogoUrl
            };
        }
    }
}