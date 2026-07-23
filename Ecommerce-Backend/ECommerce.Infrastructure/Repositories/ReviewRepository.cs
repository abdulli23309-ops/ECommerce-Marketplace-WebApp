using ECommerce.Application.Interfaces;
using ECommerce.Domain.Entities;
using ECommerce.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Infrastructure.Repositories.Reviews
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly ECommerceDbContext _context;
        public ReviewRepository(ECommerceDbContext context) => _context = context;

        public async Task<Review?> GetByOrderItemIdAsync(Guid orderItemId)
            => await _context.Reviews
                .Include(r => r.ReviewImages)
                .FirstOrDefaultAsync(r => r.OrderItemId == orderItemId);

        public async Task<IEnumerable<Review>> GetByProductIdAsync(Guid productId)
            => await _context.Reviews
                .Include(r => r.ReviewImages)
                .Where(r => r.ProductId == productId)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();

        public async Task<IEnumerable<Review>> GetByUserIdAsync(Guid userId)
            => await _context.Reviews
                .Include(r => r.ReviewImages)
                .Where(r => r.UserId == userId)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();

        public async Task AddAsync(Review review)
            => await _context.Reviews.AddAsync(review);

        public async Task SaveChangesAsync() => await _context.SaveChangesAsync();
    }
}