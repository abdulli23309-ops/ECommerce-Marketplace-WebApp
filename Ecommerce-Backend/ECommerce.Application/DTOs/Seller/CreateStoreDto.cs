namespace ECommerce.Application.DTOs.Seller
{
    public class CreateStoreDto
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? LogoUrl { get; set; }
    }
}