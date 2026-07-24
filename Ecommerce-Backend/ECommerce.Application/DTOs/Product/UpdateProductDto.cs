namespace ECommerce.Application.DTOs.Product
{
    public class UpdateProductDto
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal BasePrice { get; set; }
        public int StockQuantity { get; set; }
        public Guid? SubCategoryId { get; set; }
        public Guid? BrandId { get; set; }
    }
}