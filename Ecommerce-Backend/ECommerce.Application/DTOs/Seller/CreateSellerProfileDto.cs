namespace ECommerce.Application.DTOs.Seller
{
    public class CreateSellerProfileDto
    {
        public string BusinessName { get; set; } = string.Empty;
        public string? Description { get; set; }
    }
}