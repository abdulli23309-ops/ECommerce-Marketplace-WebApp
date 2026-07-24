using ECommerce.Domain.Enums;

namespace ECommerce.Domain.Entities
{
    public class ReturnRequest
    {
        public Guid Id { get; set; }
        public Guid OrderItemId { get; set; }
        public OrderItem OrderItem { get; set; } = null!;
        public string Reason { get; set; } = string.Empty;
        public string? Description { get; set; }
        public ReturnStatus Status { get; set; } = ReturnStatus.Requested;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        public ICollection<ReturnImage> ReturnImages { get; set; } = new List<ReturnImage>();
    }
}