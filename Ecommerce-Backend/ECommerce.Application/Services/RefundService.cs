using ECommerce.Application.DTOs.Refunds;
using ECommerce.Application.Interfaces;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Enums;

namespace ECommerce.Application.Services.Refunds
{
    public class RefundService : IRefundService
    {
        private readonly IRefundRepository _refundRepo;
        private readonly IPaymentRepository _paymentRepo;
        private readonly IReturnRepository _returnRepo;

        public RefundService(IRefundRepository refundRepo, IPaymentRepository paymentRepo, IReturnRepository returnRepo)
        {
            _refundRepo = refundRepo;
            _paymentRepo = paymentRepo;
            _returnRepo = returnRepo;
        }

        public async Task<RefundDto> CreateRefundAsync(CreateRefundDto dto)
        {
            var payment = await _paymentRepo.GetPaymentByIdAsync(dto.PaymentId)
              ?? throw new InvalidOperationException("Payment not found.");

            var returnRequest = await _returnRepo.GetByIdAsync(dto.ReturnRequestId)
                               ?? throw new InvalidOperationException("Return request not found.");

            if (returnRequest.Status != ReturnStatus.Approved)
                throw new InvalidOperationException("Return request must be approved before refund.");

            if (dto.Amount > payment.Amount)
                throw new InvalidOperationException("Refund amount exceeds payment amount.");

            var existingRefund = await _refundRepo.GetByReturnRequestIdAsync(dto.ReturnRequestId);
            if (existingRefund != null)
                throw new InvalidOperationException("A refund already exists for this return request.");

            var refund = new Refund
            {
                PaymentId = dto.PaymentId,
                ReturnRequestId = dto.ReturnRequestId,
                Amount = dto.Amount,
                Status = RefundStatus.Completed, // dummy
                CreatedAt = DateTime.UtcNow
            };

            await _refundRepo.AddAsync(refund);
            await _refundRepo.SaveChangesAsync();

            return new RefundDto
            {
                Id = refund.Id,
                PaymentId = refund.PaymentId,
                ReturnRequestId = refund.ReturnRequestId,
                Amount = refund.Amount,
                Status = refund.Status.ToString(),
                CreatedAt = refund.CreatedAt
            };
        }

        public async Task<RefundDto?> GetRefundByIdAsync(Guid refundId)
        {
            var refund = await _refundRepo.GetByIdAsync(refundId);
            if (refund == null) return null;
            return new RefundDto
            {
                Id = refund.Id,
                PaymentId = refund.PaymentId,
                ReturnRequestId = refund.ReturnRequestId,
                Amount = refund.Amount,
                Status = refund.Status.ToString(),
                CreatedAt = refund.CreatedAt
            };
        }
    }
}