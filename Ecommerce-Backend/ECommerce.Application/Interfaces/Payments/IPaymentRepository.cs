using ECommerce.Application.Helpers;
using ECommerce.Domain.Entities;

namespace ECommerce.Application.Interfaces
{
    public interface IPaymentRepository
    {
        Task<Payment?> GetByOrderIdAsync(Guid parentOrderId);
        Task AddAsync(Payment payment);
        void Update(Payment payment);
        Task SaveChangesAsync();
        Task<IEnumerable<Payment>> GetAllAsync();
        Task<decimal> GetTotalRevenueAsync();
        Task<PagedResult<Payment>> GetPagedAsync(int page, int pageSize, string? search = null, string? status = null, string? method = null);
        Task<Payment?> GetPaymentByIdAsync(Guid paymentId);
    }
}