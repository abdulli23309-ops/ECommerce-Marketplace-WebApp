using ECommerce.Application.DTOs.Order;

namespace ECommerce.Application.Interfaces
{
    public interface IOrderService
    {
        Task<ParentOrderDto> CheckoutAsync(Guid userId, CheckoutDto dto);
        Task<IEnumerable<ParentOrderDto>> GetMyOrdersAsync(Guid userId);
        Task<ParentOrderDto?> GetOrderByIdAsync(Guid userId, Guid parentOrderId);
        Task<IEnumerable<ParentOrderDto>> GetSellerOrdersAsync(Guid userId);
    }
}