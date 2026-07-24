using ECommerce.Domain.Entities;

namespace ECommerce.Application.Interfaces
{
    public interface IRefundRepository
    {
        Task<Refund?> GetByIdAsync(Guid refundId);
        Task<Refund?> GetByReturnRequestIdAsync(Guid returnRequestId);
        Task AddAsync(Refund refund);
        Task SaveChangesAsync();
    }
}