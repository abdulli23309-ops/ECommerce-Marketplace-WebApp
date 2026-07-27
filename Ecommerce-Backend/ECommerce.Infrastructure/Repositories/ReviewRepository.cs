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
        .Include(r => r.OrderItem)
            .ThenInclude(oi => oi.SellerOrder)
        .Where(r => r.UserId == userId)
        .OrderByDescending(r => r.CreatedAt)
        .ToListAsync();
        public async Task<IEnumerable<Review>> GetByStoreIdAsync(Guid storeId, int page, int pageSize)
    => await _context.Reviews
        .Include(r => r.ReviewImages)
        .Include(r => r.User)
        .Include(r => r.Product)
        .Where(r => r.Product!.StoreId == storeId)
        .OrderByDescending(r => r.CreatedAt)
        .Skip((page - 1) * pageSize)
        .Take(pageSize)
        .ToListAsync();
        public async Task<int> GetCountByStoreIdAsync(Guid storeId)
    => await _context.Reviews.CountAsync(r => r.Product!.StoreId == storeId);

        public async Task AddAsync(Review review)
            => await _context.Reviews.AddAsync(review);
        public async Task<double?> GetAverageRatingByStoreIdAsync(Guid storeId)
        {
            var ratings = await _context.Reviews
                .Include(r => r.Product)
                .Where(r => r.Product!.StoreId == storeId)
                .Select(r => r.Rating)
                .ToListAsync();

            return ratings.Any() ? ratings.Average() : (double?)null;
        }
        public async Task<Review?> GetByIdAsync(Guid reviewId)
    => await _context.Reviews
        .Include(r => r.ReviewImages)
        .Include(r => r.OrderItem)
            .ThenInclude(oi => oi.SellerOrder)
                .ThenInclude(so => so.ParentOrder)
        .FirstOrDefaultAsync(r => r.Id == reviewId);

        public async Task SaveChangesAsync() => await _context.SaveChangesAsync();
    }
}