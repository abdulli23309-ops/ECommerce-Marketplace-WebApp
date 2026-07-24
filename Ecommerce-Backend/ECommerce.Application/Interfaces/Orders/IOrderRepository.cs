using ECommerce.Domain.Entities;

namespace ECommerce.Application.Interfaces
{
    public interface IOrderRepository
    {
        Task AddParentOrderAsync(ParentOrder order);
        Task AddSellerOrderAsync(SellerOrder order);
        Task AddOrderItemAsync(OrderItem item);
        Task SaveChangesAsync();
        Task<IEnumerable<ParentOrder>> GetOrdersByUserIdAsync(Guid userId);
        Task<IEnumerable<ParentOrder>> GetAllAsync();

        Task<OrderItem?> GetOrderItemByIdAsync(Guid orderItemId);
    }
}