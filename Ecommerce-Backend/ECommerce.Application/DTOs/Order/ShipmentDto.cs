namespace ECommerce.Application.DTOs.Orders
{
    public class ShipmentDto
    {
        public Guid ShipmentId { get; set; }
        public Guid SellerOrderId { get; set; }
        public string? TrackingNumber { get; set; }
        public string? Carrier { get; set; }
        public string Status { get; set; } = string.Empty;
        public List<ShipmentTrackingHistoryDto> TrackingHistory { get; set; } = new();
    }
}