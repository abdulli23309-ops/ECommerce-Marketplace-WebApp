using ECommerce.Application.DTOs.Admin;
using ECommerce.Application.DTOs.Refunds;

namespace ECommerce.Application.Interfaces
{
    public interface IAdminService
    {
        // Sellers
        Task<IEnumerable<SellerAdminDto>> GetSellersAsync();
        Task ApproveSellerAsync(Guid sellerId);

        // Products
        Task<IEnumerable<ProductAdminDto>> GetProductsAsync();
        Task UpdateProductStatusAsync(Guid productId, string status);

        // Orders
        Task<IEnumerable<ParentOrderAdminDto>> GetOrdersAsync();
        Task<IEnumerable<ShipmentAdminDto>> GetShipmentsAsync();

        // Returns
        Task<IEnumerable<ReturnRequestAdminDto>> GetReturnsAsync();
        Task RejectSellerAsync(Guid sellerId, string? reason);
        Task ApproveReturnAsync(Guid returnId);
        Task RejectReturnAsync(Guid returnId);

        Task<AdminStatsDto> GetStatsAsync();

        // Refunds (existing service used directly in controller)
    }
}