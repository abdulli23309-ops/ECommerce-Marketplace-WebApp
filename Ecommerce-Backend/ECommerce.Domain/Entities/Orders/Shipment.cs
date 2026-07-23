namespace ECommerce.Domain.Entities
{
    public class Shipment
    {
        public Guid Id { get; set; }
        public Guid SellerOrderId { get; set; }
        public SellerOrder SellerOrder { get; set; } = null!;
        public string? TrackingNumber { get; set; }
        public string? Carrier { get; set; }
        public string Status { get; set; } = "Pending"; // Pending, Packed, Dispatched, OutForDelivery, Delivered
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        public ICollection<ShipmentTrackingHistory> TrackingHistories { get; set; } = new List<ShipmentTrackingHistory>();
    }
}