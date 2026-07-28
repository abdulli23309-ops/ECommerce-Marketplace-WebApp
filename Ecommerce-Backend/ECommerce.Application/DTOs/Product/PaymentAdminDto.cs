namespace ECommerce.Application.DTOs.Payment
{
    public class PaymentAdminDto
    {
        public Guid PaymentId { get; set; }
        public Guid OrderId { get; set; }
        public string CustomerEmail { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public string Method { get; set; } = string.Empty;
    }
}