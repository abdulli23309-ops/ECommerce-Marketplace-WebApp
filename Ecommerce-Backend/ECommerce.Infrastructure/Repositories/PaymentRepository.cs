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

        public async Task SaveChangesAsync() => await _context.SaveChangesAsync();
    }
}