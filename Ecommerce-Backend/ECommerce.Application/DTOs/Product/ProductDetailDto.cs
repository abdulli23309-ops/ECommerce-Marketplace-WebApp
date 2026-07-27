namespace ECommerce.Application.DTOs.Product
{
    public class ProductDetailDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal BasePrice { get; set; }
        public int StockQuantity { get; set; }
        public string Status { get; set; } = string.Empty;
        public List<ProductImageDto> Images { get; set; } = new();
        public Guid? BrandId { get; set; }
        public string? BrandName { get; set; }
        public Guid? SubCategoryId { get; set; }
        public string? SubCategoryName { get; set; }
        public Guid? CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public Guid StoreId { get; set; }
        public string? StoreLogoUrl { get; set; }
        public string? StoreDescription { get; set; }
        public string StoreName { get; set; } = string.Empty;
        public double? AverageRating { get; set; }
        public int ReviewCount { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}