using ECommerce.Domain.Entities;

namespace ECommerce.Application.Interfaces
{
    public interface IReviewRepository
    {
        Task<Review?> GetByOrderItemIdAsync(Guid orderItemId);
        Task<IEnumerable<Review>> GetByProductIdAsync(Guid productId);
        Task<IEnumerable<Review>> GetByUserIdAsync(Guid userId);
        Task<IEnumerable<Review>> GetByStoreIdAsync(Guid storeId, int page, int pageSize);
        Task<double?> GetAverageRatingByStoreIdAsync(Guid storeId);
        Task<int> GetCountByStoreIdAsync(Guid storeId);
        Task<Review?> GetByIdAsync(Guid reviewId);
        Task AddAsync(Review review);
        Task SaveChangesAsync();
    }
}