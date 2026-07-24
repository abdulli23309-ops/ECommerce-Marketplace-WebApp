namespace ECommerce.Application.DTOs.Admin
{
    public class ParentOrderAdminDto
    {
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
        public string CustomerEmail { get; set; } = string.Empty;
        public DateTime OrderDate { get; set; }
        public string OrderStatus { get; set; } = string.Empty;
        public decimal TotalAmount { get; set; }
        public List<SellerOrderAdminDto> SellerOrders { get; set; } = new();
    }

    public class SellerOrderAdminDto
    {
        public Guid Id { get; set; }
        public Guid? StoreId { get; set; }
        public string StoreName { get; set; } = string.Empty;
        public decimal SubTotal { get; set; }
        public string Status { get; set; } = string.Empty;
    }
}