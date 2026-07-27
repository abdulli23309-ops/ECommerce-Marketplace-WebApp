namespace ECommerce.Application.DTOs.Seller
{
    public class StorePublicInfoDto
    {
        public Guid StoreId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? LogoUrl { get; set; }
    }
}