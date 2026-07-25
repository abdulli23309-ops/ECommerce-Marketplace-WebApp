namespace ECommerce.Application.DTOs.Product
{
    public class ProductImageDto
    {
        public Guid Id { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public int SortOrder { get; set; }
    }
}