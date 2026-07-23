namespace ECommerce.Application.DTOs.Product
{
    public class CreateProductDto
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal BasePrice { get; set; }
        public Guid? SubCategoryId { get; set; }
    }
}