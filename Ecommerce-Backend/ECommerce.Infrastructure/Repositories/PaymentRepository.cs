using ECommerce.Application.Helpers;
using ECommerce.Application.Interfaces;
using ECommerce.Domain.Entities;
using ECommerce.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Infrastructure.Repositories
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly ECommerceDbContext _context;
        public PaymentRepository(ECommerceDbContext context) => _context = context;

        public async Task<Payment?> GetByOrderIdAsync(Guid parentOrderId)
            => await _context.Payments.FirstOrDefaultAsync(p => p.ParentOrderId == parentOrderId);

        public async Task AddAsync(Payment payment)
            => await _context.Payments.AddAsync(payment);

        public void Update(Payment payment)
            => _context.Payments.Update(payment);
        public async Task<Payment?> GetPaymentByIdAsync(Guid paymentId)
    => await _context.Payments.FindAsync(paymentId);

        public async Task<IEnumerable<Payment>> GetAllAsync()
    => await _context.Payments
        .Include(p => p.ParentOrder)
            .ThenInclude(po => po.Customer)
        .ToListAsync();
        public async Task<decimal> GetTotalRevenueAsync()
    => await _context.Payments.SumAsync(p => p.Amount);
        public async Task<PagedResult<Payment>> GetPagedAsync(int page, int pageSize, string? search = null, string? status = null, string? method = null)
        {
            var query = _context.Payments.Include(p => p.ParentOrder.Customer).AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                var term = search.Trim().ToLower();
                query = query.Where(p => p.Id.ToString().ToLower().Contains(term)
                                          || (p.ParentOrder != null && p.ParentOrder.Customer.Email.ToLower().Contains(term)));
            }
            if (!string.IsNullOrWhiteSpace(status))
                query = query.Where(p => p.Status == status);
            if (!string.IsNullOrWhiteSpace(method))
                query = query.Where(p => p.Method == method);

            query = query.OrderByDescending(p => p.CreatedAt);
            return await query.ToPagedResultAsync(page, pageSize);
        }

        public async Task SaveChangesAsync() => await _context.SaveChangesAsync();
    }
}