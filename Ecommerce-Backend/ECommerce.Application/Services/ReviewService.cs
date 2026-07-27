using ECommerce.Application.DTOs.Reviews;
using ECommerce.Application.Interfaces;
using ECommerce.Domain.Entities;

namespace ECommerce.Application.Services.Reviews
{
    public class ReviewService : IReviewService
    {
        private readonly IReviewRepository _reviewRepo;
        private readonly IOrderRepository _orderRepo;
        private readonly IShipmentRepository _shipmentRepo;
        private readonly IProductRepository _productRepo;
        private readonly IUserRepository _userRepo;

        public ReviewService(
            IReviewRepository reviewRepo,
            IOrderRepository orderRepo,
            IShipmentRepository shipmentRepo,
            IProductRepository productRepo,
            IUserRepository userRepo)
        {
            _reviewRepo = reviewRepo;
            _orderRepo = orderRepo;
            _shipmentRepo = shipmentRepo;
            _productRepo = productRepo;
            _userRepo = userRepo;
        }

        public async Task<ReviewDto> CreateReviewAsync(Guid userId, CreateReviewDto dto)
        {
            // 1. Validate rating range
            if (dto.Rating < 1 || dto.Rating > 5)
                throw new InvalidOperationException("Rating must be between 1 and 5.");

            // 2. Fetch OrderItem
            var orderItem = await _orderRepo.GetOrderItemByIdAsync(dto.OrderItemId)
                            ?? throw new InvalidOperationException("Order item not found.");

            if (orderItem.SellerOrder.ParentOrder.CustomerId != userId)
                throw new InvalidOperationException("You can only review your own purchased items.");
            // 4. Check if already reviewed
            var existingReview = await _reviewRepo.GetByOrderItemIdAsync(dto.OrderItemId);
            if (existingReview != null)
                throw new InvalidOperationException("You have already reviewed this item.");

            // 5. Check shipment delivery status
            var shipment = await _shipmentRepo.GetBySellerOrderIdAsync(orderItem.SellerOrderId);
            if (shipment == null || shipment.Status != "Delivered")
                throw new InvalidOperationException("You can only review items that have been delivered.");

            // 6. Create Review entity
            var review = new Review
            {
                OrderItemId = dto.OrderItemId,
                ProductId = orderItem.ProductId ?? Guid.Empty, // if product was deleted, ProductId might be null; we handle optional FK
                UserId = userId,
                Rating = dto.Rating,
                Comment = dto.Comment,
                CreatedAt = DateTime.UtcNow
            };

            // 7. Add images if provided
            if (dto.ImageUrls != null)
            {
                foreach (var url in dto.ImageUrls)
                {
                    review.ReviewImages.Add(new ReviewImage { ImageUrl = url });
                }
            }

            await _reviewRepo.AddAsync(review);
            await _reviewRepo.SaveChangesAsync();

            return await MapToDto(review);
        }

        public async Task<IEnumerable<ReviewDto>> GetProductReviewsAsync(Guid productId)
        {
            var reviews = await _reviewRepo.GetByProductIdAsync(productId);
            var dtos = new List<ReviewDto>();
            foreach (var r in reviews)
            {
                dtos.Add(await MapToDto(r));
            }
            return dtos;
        }

        public async Task<IEnumerable<ReviewDto>> GetMyReviewsAsync(Guid userId)
        {
            var reviews = await _reviewRepo.GetByUserIdAsync(userId);
            var dtos = new List<ReviewDto>();
            foreach (var r in reviews)
            {
                dtos.Add(await MapToDto(r));
            }
            return dtos;
        }
        public async Task<ReviewDto?> GetReviewByIdAsync(Guid reviewId)
        {
            var review = await _reviewRepo.GetByIdAsync(reviewId);
            if (review == null) return null;
            return await MapToDto(review);
        }

        private async Task<ReviewDto> MapToDto(Review review)
        {
            var product = await _productRepo.GetByIdAsync(review.ProductId ?? Guid.Empty);
            var user = await _userRepo.GetByIdAsync(review.UserId);

            return new ReviewDto
            {
                Id = review.Id,
                OrderId = review.OrderItem?.SellerOrder?.ParentOrderId,
                ProductId = review.ProductId ?? Guid.Empty,
                ProductName = product?.Name ?? "Deleted Product",
                UserName = user?.FullName ?? "Unknown User",
                Rating = review.Rating,
                Comment = review.Comment,
                CreatedAt = review.CreatedAt,
                Images = review.ReviewImages.Select(ri => new ReviewImageDto
                {
                    ImageUrl = ri.ImageUrl
                }).ToList()
            };
        }
    }
}