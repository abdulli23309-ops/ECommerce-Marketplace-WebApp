using ECommerce.Application.DTOs.Orders;

namespace ECommerce.Application.Interfaces
{
    public interface IShipmentService
    {
        Task<ShipmentDto> CreateShipmentAsync(CreateShipmentDto dto);
        Task<ShipmentDto> UpdateShipmentStatusAsync(Guid shipmentId, UpdateShipmentStatusDto dto);
        Task<ShipmentDto?> GetShipmentByOrderAsync(Guid sellerOrderId);
    }
}