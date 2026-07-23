using ECommerce.Application.DTOs.Reviews;

namespace ECommerce.Application.Interfaces
{
    public interface IReviewService
    {
        Task<ReviewDto> CreateReviewAsync(Guid userId, CreateReviewDto dto);
        Task<IEnumerable<ReviewDto>> GetProductReviewsAsync(Guid productId);
        Task<IEnumerable<ReviewDto>> GetMyReviewsAsync(Guid userId);
    }
}