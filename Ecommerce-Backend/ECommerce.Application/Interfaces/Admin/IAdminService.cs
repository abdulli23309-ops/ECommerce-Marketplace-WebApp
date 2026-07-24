using ECommerce.Application.DTOs.Admin;
using ECommerce.Application.DTOs.Refunds;

namespace ECommerce.Application.Interfaces
{
    public interface IAdminService
    {
        // Sellers
        Task<IEnumerable<SellerAdminDto>> GetSellersAsync();
        Task ApproveSellerAsync(Guid sellerId);
        Task RejectSellerAsync(Guid sellerId);

        // Products
        Task<IEnumerable<ProductAdminDto>> GetProductsAsync();
        Task UpdateProductStatusAsync(Guid productId, string status);

        // Orders
        Task<IEnumerable<ParentOrderAdminDto>> GetOrdersAsync();
        Task<IEnumerable<ShipmentAdminDto>> GetShipmentsAsync();

        // Returns
        Task<IEnumerable<ReturnRequestAdminDto>> GetReturnsAsync();
        Task ApproveReturnAsync(Guid returnId);
        Task RejectReturnAsync(Guid returnId);

        // Refunds (existing service used directly in controller)
    }
}