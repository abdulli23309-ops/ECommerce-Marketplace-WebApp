namespace ECommerce.Application.DTOs.Refunds
{
    public class RefundDto
    {
        public Guid Id { get; set; }
        public Guid PaymentId { get; set; }
        public Guid ReturnRequestId { get; set; }
        public decimal Amount { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}