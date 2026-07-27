using ECommerce.Domain.Entities;

namespace ECommerce.Application.Interfaces
{
    public interface IShipmentRepository
    {
        Task<Shipment?> GetBySellerOrderIdAsync(Guid sellerOrderId);
        Task AddAsync(Shipment shipment);
        void Update(Shipment shipment);
        Task AddTrackingHistoryAsync(ShipmentTrackingHistory history);
        Task SaveChangesAsync();
        Task<Shipment?> GetByIdAsync(Guid shipmentId);
        Task<int> GetPendingShipmentsCountByStoreIdAsync(Guid storeId);
        Task<IEnumerable<Shipment>> GetAllAsync();
    }
}