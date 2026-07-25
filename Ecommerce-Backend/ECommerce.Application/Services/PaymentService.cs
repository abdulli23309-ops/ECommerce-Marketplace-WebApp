using ECommerce.Application.DTOs.Payment;
using ECommerce.Application.Interfaces;
using ECommerce.Domain.Entities;

namespace ECommerce.Application.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _paymentRepo;
        private readonly IOrderRepository _orderRepo;

        public PaymentService(IPaymentRepository paymentRepo, IOrderRepository orderRepo)
        {
            _paymentRepo = paymentRepo;
            _orderRepo = orderRepo;
        }

        public async Task<PaymentDto> MakePaymentAsync(Guid userId, MakePaymentDto dto)
        {
            // Verify the order belongs to the user
            var orders = await _orderRepo.GetOrdersByUserIdAsync(userId);
            var order = orders.FirstOrDefault(o => o.Id == dto.OrderId)
                        ?? throw new InvalidOperationException("Order not found or does not belong to user.");

            if (order.Payment != null)
                throw new InvalidOperationException("Payment already exists for this order.");

            var payment = new Payment
            {
                ParentOrderId = dto.OrderId,
                Amount = order.TotalAmount,
                Method = dto.Method,
                Status = "Completed", // For simplicity, mark as completed immediately (no gateway)
                CreatedAt = DateTime.UtcNow
            };

            await _paymentRepo.AddAsync(payment);
            await _paymentRepo.SaveChangesAsync();

            return new PaymentDto
            {
                PaymentId = payment.Id,
                OrderId = payment.ParentOrderId,
                Amount = payment.Amount,
                Method = payment.Method,
                Status = payment.Status
            };
        }

        public async Task<PaymentDto?> GetPaymentStatusAsync(Guid userId, Guid orderId)
        {
            var orders = await _orderRepo.GetOrdersByUserIdAsync(userId);
            var order = orders.FirstOrDefault(o => o.Id == orderId);
            if (order?.Payment == null) return null;

            return new PaymentDto
            {
                PaymentId = order.Payment.Id,
                OrderId = order.Payment.ParentOrderId,
                Amount = order.Payment.Amount,
                Method = order.Payment.Method,
                Status = order.Payment.Status
            };
        }
        public async Task<IEnumerable<PaymentAdminDto>> GetAllPaymentsAsync()
        {
            var payments = await _paymentRepo.GetAllAsync();
            return payments.Select(p => new PaymentAdminDto
            {
                PaymentId = p.Id,
                OrderId = p.ParentOrderId,
                CustomerEmail = p.ParentOrder?.Customer?.Email ?? "Unknown",
                Amount = p.Amount,
                Status = p.Status,
                Method = p.Method
            });
        }
    }
}