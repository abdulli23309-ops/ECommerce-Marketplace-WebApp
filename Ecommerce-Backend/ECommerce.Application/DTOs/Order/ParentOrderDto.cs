namespace ECommerce.Application.DTOs.Order
{
    public class ParentOrderDto
    {
        public Guid ParentOrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public string OrderStatus { get; set; } = string.Empty;
        public decimal TotalAmount { get; set; }
        public List<SellerOrderDto> SellerOrders { get; set; } = new();
    }
}