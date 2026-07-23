using ECommerce.Application.DTOs.Orders;
using ECommerce.Application.Interfaces;
using ECommerce.Domain.Entities;

namespace ECommerce.Application.Services.Orders
{
    public class ShipmentService : IShipmentService
    {
        private readonly IShipmentRepository _repo;
        public ShipmentService(IShipmentRepository repo) => _repo = repo;

        public async Task<ShipmentDto> CreateShipmentAsync(CreateShipmentDto dto)
        {
            var shipment = new Shipment
            {
                SellerOrderId = dto.SellerOrderId,
                TrackingNumber = dto.TrackingNumber,
                Carrier = dto.Carrier,
                Status = "Pending",
                CreatedAt = DateTime.UtcNow
            };
            await _repo.AddAsync(shipment);
            await _repo.SaveChangesAsync();

            return MapToDto(shipment);
        }

        public async Task<ShipmentDto> UpdateShipmentStatusAsync(Guid shipmentId, UpdateShipmentStatusDto dto)
        {
            var shipment = await _repo.GetByIdAsync(shipmentId)
                           ?? throw new InvalidOperationException("Shipment not found.");

            shipment.Status = dto.Status;
            shipment.UpdatedAt = DateTime.UtcNow;
            _repo.Update(shipment);

            var history = new ShipmentTrackingHistory
            {
                ShipmentId = shipment.Id,
                Status = dto.Status,
                Location = dto.Location,
                Timestamp = DateTime.UtcNow
            };
            await _repo.AddTrackingHistoryAsync(history);
            await _repo.SaveChangesAsync();

            return MapToDto(shipment);
        }

        public async Task<ShipmentDto?> GetShipmentByOrderAsync(Guid sellerOrderId)
        {
            var shipment = await _repo.GetBySellerOrderIdAsync(sellerOrderId);
            return shipment == null ? null : MapToDto(shipment);
        }

        private ShipmentDto MapToDto(Shipment shipment)
        {
            return new ShipmentDto
            {
                ShipmentId = shipment.Id,
                SellerOrderId = shipment.SellerOrderId,
                TrackingNumber = shipment.TrackingNumber,
                Carrier = shipment.Carrier,
                Status = shipment.Status,
                TrackingHistory = shipment.TrackingHistories.Select(h => new ShipmentTrackingHistoryDto
                {
                    Status = h.Status,
                    Location = h.Location,
                    Timestamp = h.Timestamp
                }).ToList()
            };
        }
    }
}