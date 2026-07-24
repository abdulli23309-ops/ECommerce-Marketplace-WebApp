using ECommerce.Domain.Enums;

namespace ECommerce.Domain.Entities
{
    public class Refund
    {
        public Guid Id { get; set; }
        public Guid PaymentId { get; set; }
        public Payment Payment { get; set; } = null!;
        public Guid ReturnRequestId { get; set; }
        public ReturnRequest ReturnRequest { get; set; } = null!;
        public decimal Amount { get; set; }
        public RefundStatus Status { get; set; } = RefundStatus.Completed;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }
}