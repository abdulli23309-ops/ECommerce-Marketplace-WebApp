using ECommerce.Domain.Entities;

namespace ECommerce.Application.Interfaces
{
    public interface IReviewRepository
    {
        Task<Review?> GetByOrderItemIdAsync(Guid orderItemId);
        Task<IEnumerable<Review>> GetByProductIdAsync(Guid productId);
        Task<IEnumerable<Review>> GetByUserIdAsync(Guid userId);
        Task AddAsync(Review review);
        Task SaveChangesAsync();
    }
}