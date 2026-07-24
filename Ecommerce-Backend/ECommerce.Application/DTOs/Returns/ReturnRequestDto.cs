namespace ECommerce.Application.DTOs.Returns
{
    public class ReturnRequestDto
    {
        public Guid Id { get; set; }
        public Guid OrderItemId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string Reason { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public List<ReturnImageDto> Images { get; set; } = new();
    }
}