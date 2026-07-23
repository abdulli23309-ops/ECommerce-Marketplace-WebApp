namespace ECommerce.Application.DTOs.Order
{
    public class OrderItemDto
    {
        public string ProductName { get; set; } = string.Empty;
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
    }
}