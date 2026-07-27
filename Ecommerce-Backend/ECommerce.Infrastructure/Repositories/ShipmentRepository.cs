using ECommerce.Application.Interfaces;
using ECommerce.Domain.Entities;
using ECommerce.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Infrastructure.Repositories.Orders
{
    public class ShipmentRepository : IShipmentRepository
    {
        private readonly ECommerceDbContext _context;
        public ShipmentRepository(ECommerceDbContext context) => _context = context;

        public async Task<Shipment?> GetBySellerOrderIdAsync(Guid sellerOrderId)
            => await _context.Shipments
                .Include(s => s.TrackingHistories.OrderBy(h => h.Timestamp))
                .FirstOrDefaultAsync(s => s.SellerOrderId == sellerOrderId);

        public async Task AddAsync(Shipment shipment)
            => await _context.Shipments.AddAsync(shipment);

        public void Update(Shipment shipment)
            => _context.Shipments.Update(shipment);

        public async Task AddTrackingHistoryAsync(ShipmentTrackingHistory history)
            => await _context.ShipmentTrackingHistories.AddAsync(history);

        public async Task SaveChangesAsync() => await _context.SaveChangesAsync();
        public async Task<Shipment?> GetByIdAsync(Guid shipmentId)
            => await _context.Shipments
        .Include(s => s.TrackingHistories.OrderBy(h => h.Timestamp))
        .Include(s => s.SellerOrder)
        .FirstOrDefaultAsync(s => s.Id == shipmentId);
        public async Task<IEnumerable<Shipment>> GetAllAsync()
    => await _context.Shipments.ToListAsync();
        public async Task<int> GetPendingShipmentsCountByStoreIdAsync(Guid storeId)
    => await _context.Shipments
        .Where(s => s.SellerOrder.StoreId == storeId && s.Status != "Delivered")
        .CountAsync();
    }
}