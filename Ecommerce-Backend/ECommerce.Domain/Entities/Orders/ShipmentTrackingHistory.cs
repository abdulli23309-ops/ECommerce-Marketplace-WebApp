namespace ECommerce.Domain.Entities
{
    public class ShipmentTrackingHistory
    {
        public Guid Id { get; set; }
        public Guid ShipmentId { get; set; }
        public Shipment Shipment { get; set; } = null!;
        public string Status { get; set; } = string.Empty;
        public string? Location { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}