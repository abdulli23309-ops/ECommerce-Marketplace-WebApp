using ECommerce.Application.Interfaces;
using ECommerce.Domain.Entities;
using ECommerce.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Infrastructure.Repositories.Returns
{
    public class ReturnRepository : IReturnRepository
    {
        private readonly ECommerceDbContext _context;
        public ReturnRepository(ECommerceDbContext context) => _context = context;

        public async Task<ReturnRequest?> GetByOrderItemIdAsync(Guid orderItemId)
            => await _context.ReturnRequests
                .Include(r => r.ReturnImages)
                .FirstOrDefaultAsync(r => r.OrderItemId == orderItemId);

        public async Task<ReturnRequest?> GetByIdAsync(Guid returnRequestId)
            => await _context.ReturnRequests
            .Include(r => r.ReturnImages)
             .Include(r => r.OrderItem) // for product name snapshot
              .FirstOrDefaultAsync(r => r.Id == returnRequestId);

        public async Task<IEnumerable<ReturnRequest>> GetByUserIdAsync(Guid userId)
            => await _context.ReturnRequests
                .Include(r => r.ReturnImages)
                .Include(r => r.OrderItem)
                .Where(r => r.OrderItem.SellerOrder.ParentOrder.CustomerId == userId) // navigation path
                .ToListAsync();

        public async Task AddAsync(ReturnRequest returnRequest)
            => await _context.ReturnRequests.AddAsync(returnRequest);
        public async Task<IEnumerable<ReturnRequest>> GetAllAsync()
        => await _context.ReturnRequests
            .Include(r => r.ReturnImages)
            .Include(r => r.OrderItem)
    .ThenInclude(oi => oi.SellerOrder)
        .ThenInclude(so => so.ParentOrder)
            .ThenInclude(po => po.Customer)
            .ToListAsync();
        public void Update(ReturnRequest returnRequest)
    => _context.ReturnRequests.Update(returnRequest);

        public async Task SaveChangesAsync() => await _context.SaveChangesAsync();
    }
}