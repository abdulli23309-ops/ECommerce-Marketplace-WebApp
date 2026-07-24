using ECommerce.Application.DTOs.Refunds;

namespace ECommerce.Application.Interfaces
{
    public interface IRefundService
    {
        Task<RefundDto> CreateRefundAsync(CreateRefundDto dto);
        Task<RefundDto?> GetRefundByIdAsync(Guid refundId);
    }
}