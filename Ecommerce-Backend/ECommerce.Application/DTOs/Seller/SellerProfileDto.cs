namespace ECommerce.Application.DTOs.Seller
{
    public class SellerProfileDto
    {
        public Guid Id { get; set; }
        public string BusinessName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string Status { get; set; } = string.Empty;
        public string? RejectionReason { get; set; }   // <-- add this
    }
}