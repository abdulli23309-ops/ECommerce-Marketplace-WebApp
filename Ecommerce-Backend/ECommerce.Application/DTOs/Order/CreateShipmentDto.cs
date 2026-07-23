namespace ECommerce.Application.DTOs.Orders
{
    public class CreateShipmentDto
    {
        public Guid SellerOrderId { get; set; }
        public string? TrackingNumber { get; set; }
        public string? Carrier { get; set; }
    }
}