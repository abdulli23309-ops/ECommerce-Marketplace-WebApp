using ECommerce.Domain.Entities;

namespace ECommerce.Application.Interfaces
{
    public interface IPaymentRepository
    {
        Task<Payment?> GetByOrderIdAsync(Guid parentOrderId);
        Task AddAsync(Payment payment);
        void Update(Payment payment);
        Task SaveChangesAsync();
    }
}