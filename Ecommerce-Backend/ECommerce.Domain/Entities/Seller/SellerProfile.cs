

namespace ECommerce.Domain.Entities
{
    public class SellerProfile
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; } = null!;
        public string BusinessName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string Status { get; set; } = "Pending"; 
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public string? RejectionReason { get; set; }
        public ICollection<Store> Stores { get; set; } = new List<Store>();
    }
}