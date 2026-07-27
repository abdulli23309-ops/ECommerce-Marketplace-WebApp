namespace ECommerce.Application.DTOs.Admin
{
    public class SellerAdminDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Email { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string BusinessName { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string? LogoUrl { get; set; }
    }
}