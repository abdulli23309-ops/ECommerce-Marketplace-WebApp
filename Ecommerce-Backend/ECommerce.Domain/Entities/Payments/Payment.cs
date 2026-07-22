

namespace ECommerce.Domain.Entities
{
    public class Payment
    {
        public Guid Id { get; set; }
        public Guid ParentOrderId { get; set; }
        public ParentOrder ParentOrder { get; set; } = null!;
        public decimal Amount { get; set; }
        public string Method { get; set; } = string.Empty; // CashOnDelivery, CreditCard, etc.
        public string Status { get; set; } = "Pending"; // Pending, Completed, Failed, Refunded
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }
}