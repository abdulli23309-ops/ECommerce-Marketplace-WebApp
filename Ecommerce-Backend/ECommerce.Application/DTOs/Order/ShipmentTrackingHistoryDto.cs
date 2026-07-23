namespace ECommerce.Application.DTOs.Orders
{
    public class ShipmentTrackingHistoryDto
    {
        public string Status { get; set; } = string.Empty;
        public string? Location { get; set; }
        public DateTime Timestamp { get; set; }
    }
}