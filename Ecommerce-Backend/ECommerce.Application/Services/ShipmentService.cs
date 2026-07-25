using ECommerce.Application.DTOs.Orders;
using ECommerce.Application.Interfaces;
using ECommerce.Domain.Entities;

namespace ECommerce.Application.Services.Orders
{
    public class ShipmentService : IShipmentService
    {
        private readonly IShipmentRepository _repo;
        private readonly IOrderRepository _orderRepo;
        private readonly ISellerRepository _sellerRepo;

        public ShipmentService(IShipmentRepository repo, IOrderRepository orderRepo, ISellerRepository sellerRepo)
        {
            _repo = repo;
            _orderRepo = orderRepo;
            _sellerRepo = sellerRepo;
        }

        /// <summary>
        /// Resolves the calling seller's StoreId from their user id, and throws if the
        /// SellerOrder they're touching doesn't belong to that store. Admins skip this.
        /// </summary>
        private async Task EnsureCallerOwnsSellerOrderAsync(Guid sellerOrderId, Guid callerUserId, bool isAdmin)
        {
            if (isAdmin) return;

            var sellerProfile = await _sellerRepo.GetByUserIdAsync(callerUserId)
                ?? throw new UnauthorizedAccessException("No seller profile found for the current user.");

            var callerStore = await _sellerRepo.GetStoreBySellerIdAsync(sellerProfile.Id)
                ?? throw new UnauthorizedAccessException("No store found for the current seller.");

            var orderStoreId = await _orderRepo.GetStoreIdBySellerOrderIdAsync(sellerOrderId);

            if (orderStoreId == null || orderStoreId != callerStore.Id)
                throw new UnauthorizedAccessException("You do not have permission to manage this order's shipment.");
        }

        public async Task<ShipmentDto> CreateShipmentAsync(CreateShipmentDto dto, Guid callerUserId, bool isAdmin)
        {
            await EnsureCallerOwnsSellerOrderAsync(dto.SellerOrderId, callerUserId, isAdmin);

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

        public async Task<ShipmentDto> UpdateShipmentStatusAsync(Guid shipmentId, UpdateShipmentStatusDto dto, Guid callerUserId, bool isAdmin)
        {
            var shipment = await _repo.GetByIdAsync(shipmentId)
                           ?? throw new InvalidOperationException("Shipment not found.");

            await EnsureCallerOwnsSellerOrderAsync(shipment.SellerOrderId, callerUserId, isAdmin);

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