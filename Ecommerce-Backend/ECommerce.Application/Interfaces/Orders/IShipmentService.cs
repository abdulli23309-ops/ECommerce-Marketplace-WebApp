using ECommerce.Application.DTOs.Orders;

namespace ECommerce.Application.Interfaces
{
    public interface IShipmentService
    {
        // callerUserId/isAdmin were added for the Phase 0 security fix: a Seller
        // may only create/update shipments for SellerOrders belonging to their own
        // store. SuperAdmin bypasses the ownership check. isAdmin=true with a
        // Guid.Empty callerUserId is how the controller signals an admin caller.
        Task<ShipmentDto> CreateShipmentAsync(CreateShipmentDto dto, Guid callerUserId, bool isAdmin);
        Task<ShipmentDto> UpdateShipmentStatusAsync(Guid shipmentId, UpdateShipmentStatusDto dto, Guid callerUserId, bool isAdmin);
        Task<ShipmentDto?> GetShipmentByOrderAsync(Guid sellerOrderId);
    }
}