namespace ECommerce.Application.DTOs.Returns
{
    public class CreateReturnRequestDto
    {
        public Guid OrderItemId { get; set; }
        public string Reason { get; set; } = string.Empty;
        public string? Description { get; set; }
        public List<string>? ImageUrls { get; set; }
    }
}