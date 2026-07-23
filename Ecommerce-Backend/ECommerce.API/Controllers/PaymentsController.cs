using ECommerce.Application.DTOs.Payment;
using ECommerce.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ECommerce.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PaymentsController : ControllerBase
    {
        private readonly IPaymentService _paymentService;
        public PaymentsController(IPaymentService paymentService) => _paymentService = paymentService;

        private Guid GetUserId() => Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        [HttpPost]
        public async Task<IActionResult> MakePayment(MakePaymentDto dto)
            => Ok(await _paymentService.MakePaymentAsync(GetUserId(), dto));

        [HttpGet("{orderId}")]
        public async Task<IActionResult> GetPaymentStatus(Guid orderId)
        {
            var payment = await _paymentService.GetPaymentStatusAsync(GetUserId(), orderId);
            if (payment == null) return NotFound();
            return Ok(payment);
        }
    }
}