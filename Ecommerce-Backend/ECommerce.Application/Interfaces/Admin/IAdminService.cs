using ECommerce.Application.DTOs.Admin;
using ECommerce.Application.DTOs.Payment;
using ECommerce.Application.DTOs.Refunds;
using ECommerce.Application.Helpers;

namespace ECommerce.Application.Interfaces
{
    public interface IAdminService
    {
        // Sellers
        Task<IEnumerable<SellerAdminDto>> GetSellersAsync();

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
        Task ApproveSellerAsync(Guid sellerId, string? roleId = null);
        Task<PagedResult<ParentOrderAdminDto>> GetOrdersPagedAsync(int page, int pageSize, string? search = null, string? status = null, string? sortBy = null);
        Task<PagedResult<ShipmentAdminDto>> GetShipmentsPagedAsync(int page, int pageSize, string? search = null, string? status = null);
        Task<PagedResult<PaymentAdminDto>> GetPaymentsPagedAsync(int page, int pageSize, string? search = null, string? status = null, string? method = null);

        Task<AdminStatsDto> GetStatsAsync();

        // Refunds (existing service used directly in controller)
    }
}