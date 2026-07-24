namespace ECommerce.Application.DTOs.Refunds
{
    public class CreateRefundDto
    {
        public Guid PaymentId { get; set; }
        public Guid ReturnRequestId { get; set; }
        public decimal Amount { get; set; }
    }
}