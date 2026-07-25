using ECommerce.Application.DTOs.Admin;
using ECommerce.Application.DTOs.Refunds;
using ECommerce.Application.Interfaces;
using ECommerce.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "SuperAdmin")] // entire controller admin-only
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;
        private readonly IRefundService _refundService;
        private readonly IPaymentService _paymentService; 

        public AdminController(IAdminService adminService, IRefundService refundService, IPaymentService paymentService)
        {
            _adminService = adminService;
            _refundService = refundService;
            _paymentService = paymentService;
        }

        // --- Sellers ---
        [HttpGet("sellers")]
        public async Task<IActionResult> GetSellers() => Ok(await _adminService.GetSellersAsync());

        [HttpPut("sellers/{id}/approve")]
        public async Task<IActionResult> ApproveSeller(Guid id)
        {
            await _adminService.ApproveSellerAsync(id);
            return NoContent();
        }

        [HttpPut("sellers/{id}/reject")]
        public async Task<IActionResult> RejectSeller(Guid id)
        {
            await _adminService.RejectSellerAsync(id);
            return NoContent();
        }

        // --- Products ---
        [HttpGet("products")]
        public async Task<IActionResult> GetProducts() => Ok(await _adminService.GetProductsAsync());

        [HttpPut("products/{id}/status")]
        public async Task<IActionResult> UpdateProductStatus(Guid id, [FromBody] string status)
        {
            await _adminService.UpdateProductStatusAsync(id, status);
            return NoContent();
        }

        // --- Orders ---
        [HttpGet("orders")]
        public async Task<IActionResult> GetOrders() => Ok(await _adminService.GetOrdersAsync());

        // --- Shipments ---
        [HttpGet("shipments")]
        public async Task<IActionResult> GetShipments() => Ok(await _adminService.GetShipmentsAsync());

        // --- Returns ---
        [HttpGet("returns")]
        public async Task<IActionResult> GetReturns() => Ok(await _adminService.GetReturnsAsync());

        [HttpPut("returns/{id}/approve")]
        public async Task<IActionResult> ApproveReturn(Guid id)
        {
            await _adminService.ApproveReturnAsync(id);
            return NoContent();
        }

        [HttpPut("returns/{id}/reject")]
        public async Task<IActionResult> RejectReturn(Guid id)
        {
            await _adminService.RejectReturnAsync(id);
            return NoContent();
        }

        // --- Refunds (admin can also create refunds via existing RefundsController, but we can duplicate or redirect; here we just call refund service) ---
        [HttpPost("refunds")]
        public async Task<IActionResult> CreateRefund(CreateRefundDto dto)
        {
            var result = await _refundService.CreateRefundAsync(dto);
            return Ok(result);
        }
        [HttpGet("payments")]
        public async Task<IActionResult> GetPayments()
        {
            var payments = await _paymentService.GetAllPaymentsAsync();
            return Ok(payments);
        }
        [HttpGet("stats")]
        public async Task<IActionResult> GetStats()
        {
            var stats = await _adminService.GetStatsAsync();
            return Ok(stats);
        }
    }
}