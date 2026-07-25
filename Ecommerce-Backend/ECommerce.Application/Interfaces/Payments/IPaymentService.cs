using ECommerce.Application.DTOs.Payment;

namespace ECommerce.Application.Interfaces
{
    public interface IPaymentService
    {
        Task<PaymentDto> MakePaymentAsync(Guid userId, MakePaymentDto dto);
        Task<PaymentDto?> GetPaymentStatusAsync(Guid userId, Guid orderId);
        Task<IEnumerable<PaymentAdminDto>> GetAllPaymentsAsync();

    }
}