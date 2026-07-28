using ECommerce.Application.Helpers;
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
        Task<PagedResult<Shipment>> GetPagedAsync(int page, int pageSize, string? search = null, string? status = null);
        Task<int> GetPendingShipmentsCountByStoreIdAsync(Guid storeId);
        Task<IEnumerable<Shipment>> GetAllAsync();
    }
}