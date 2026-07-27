using ECommerce.Application.DTOs.Reviews;

namespace ECommerce.Application.Interfaces
{
    public interface IReviewService
    {
        Task<ReviewDto> CreateReviewAsync(Guid userId, CreateReviewDto dto);
        Task<IEnumerable<ReviewDto>> GetProductReviewsAsync(Guid productId);
        Task<ReviewDto?> GetReviewByIdAsync(Guid reviewId);
        Task<IEnumerable<ReviewDto>> GetMyReviewsAsync(Guid userId);
    }
}