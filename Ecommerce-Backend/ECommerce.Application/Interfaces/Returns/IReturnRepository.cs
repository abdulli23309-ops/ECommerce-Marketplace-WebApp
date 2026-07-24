using ECommerce.Domain.Entities;

namespace ECommerce.Application.Interfaces
{
    public interface IReturnRepository
    {
        Task<ReturnRequest?> GetByOrderItemIdAsync(Guid orderItemId);
        Task<ReturnRequest?> GetByIdAsync(Guid returnRequestId);
        Task<IEnumerable<ReturnRequest>> GetByUserIdAsync(Guid userId);
        Task AddAsync(ReturnRequest returnRequest);
        Task<IEnumerable<ReturnRequest>> GetAllAsync();
        void Update(ReturnRequest returnRequest);
        Task SaveChangesAsync();
    }
}