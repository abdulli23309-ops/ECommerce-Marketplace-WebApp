using ECommerce.Application.DTOs.Orders;

namespace ECommerce.Application.DTOs.Order
{
    public class SellerOrderDto
    {
        public Guid SellerOrderId { get; set; }
        public string StoreName { get; set; } = string.Empty;
        public decimal SubTotal { get; set; }
        public string Status { get; set; } = string.Empty;
        public ShipmentDto? Shipment { get; set; }
        public List<OrderItemDto> Items { get; set; } = new();
    }
}