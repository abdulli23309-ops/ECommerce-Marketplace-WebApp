namespace ECommerce.Application.DTOs.Product
{
    public class ProductDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal BasePrice { get; set; }
        public string Status { get; set; } = string.Empty;
        public List<string> Images { get; set; } = new();
    }
}