namespace ECommerce.Application.DTOs.Admin
{
    public class ShipmentAdminDto
    {
        public Guid Id { get; set; }
        public Guid SellerOrderId { get; set; }
        public string? TrackingNumber { get; set; }
        public string? Carrier { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}