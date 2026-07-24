using ECommerce.Application.Interfaces;
using ECommerce.Domain.Entities;
using ECommerce.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Infrastructure.Repositories.Refunds
{
    public class RefundRepository : IRefundRepository
    {
        private readonly ECommerceDbContext _context;
        public RefundRepository(ECommerceDbContext context) => _context = context;

        public async Task<Refund?> GetByIdAsync(Guid refundId)
            => await _context.Refunds
                .Include(r => r.Payment)
                .Include(r => r.ReturnRequest)
                .FirstOrDefaultAsync(r => r.Id == refundId);

        public async Task<Refund?> GetByReturnRequestIdAsync(Guid returnRequestId)
            => await _context.Refunds
                .FirstOrDefaultAsync(r => r.ReturnRequestId == returnRequestId);

        public async Task AddAsync(Refund refund)
            => await _context.Refunds.AddAsync(refund);

        public async Task SaveChangesAsync() => await _context.SaveChangesAsync();
    }
}