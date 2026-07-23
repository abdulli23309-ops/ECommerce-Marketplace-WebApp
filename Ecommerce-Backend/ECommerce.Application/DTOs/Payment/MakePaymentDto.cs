namespace ECommerce.Application.DTOs.Payment
{
    public class MakePaymentDto
    {
        public Guid OrderId { get; set; }
        public string Method { get; set; } = "CashOnDelivery"; // CashOnDelivery, CreditCard, etc.
    }
}