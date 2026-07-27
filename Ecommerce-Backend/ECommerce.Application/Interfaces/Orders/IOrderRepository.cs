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
        Task<IEnumerable<SellerOrder>> GetSellerOrdersByStoreIdAsync(Guid storeId);
        Task<int> GetOrderCountAsync();
        Task<ParentOrder?> GetOrderByIdForUserAsync(Guid parentOrderId, Guid userId);
        Task<SellerOrder?> GetSellerOrderByIdAsync(Guid sellerOrderId);
        void UpdateSellerOrder(SellerOrder order);
        Task<ParentOrder?> GetParentOrderByIdAsync(Guid parentOrderId);
        void UpdateParentOrder(ParentOrder order);
        Task<OrderItem?> GetOrderItemByIdAsync(Guid orderItemId);
        // Added for Phase 0 security fix: lets the service layer verify a seller
        // actually owns the SellerOrder they're trying to attach/update a shipment for.
        Task<Guid?> GetStoreIdBySellerOrderIdAsync(Guid sellerOrderId);
    }
}