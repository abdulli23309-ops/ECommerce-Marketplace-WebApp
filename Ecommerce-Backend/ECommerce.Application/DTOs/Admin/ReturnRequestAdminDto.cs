namespace ECommerce.Application.DTOs.Admin
{
    public class ReturnRequestAdminDto
    {
        public Guid Id { get; set; }
        public Guid OrderItemId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string Reason { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string Status { get; set; } = string.Empty;
        public Guid? CustomerId { get; set; }
        public string CustomerEmail { get; set; } = string.Empty;
    }
}