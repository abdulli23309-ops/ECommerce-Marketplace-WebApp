using ECommerce.Application.DTOs.Refunds;
using ECommerce.Application.Interfaces;
using ECommerce.Application.Services.Refunds;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // class default: any authenticated user may hit GET below
    public class RefundsController : ControllerBase
    {
        private readonly IRefundService _refundService;
        public RefundsController(IRefundService refundService) => _refundService = refundService;

        // Refund creation is a privileged, money-moving action — SuperAdmin only.
        // NOTE: this duplicates AdminController.CreateRefund. Recommend deleting this
        // POST action entirely and routing all refund creation through AdminController
        // so there is exactly one path to create a refund, not two.
        [HttpPost]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> CreateRefund(CreateRefundDto dto)
        {
            var result = await _refundService.CreateRefundAsync(dto);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetRefund(Guid id)
        {
            // KNOWN GAP (not fixed in this pass): this does not verify the caller
            // actually owns the refund being requested. ReturnRequest has no
            // CustomerId of its own — ownership can only be derived by joining
            // ReturnRequest -> OrderItem -> SellerOrder -> ParentOrder -> UserId,
            // which IRefundService/IRefundRepository don't currently support.
            // Any authenticated user can currently view any refund by ID (IDOR).
            // Do not consider Phase 0 fully closed until this is addressed —
            // either add that ownership join, or restrict this endpoint to
            // SuperAdmin + a separate "GetMyRefund" customer-scoped endpoint.
            var result = await _refundService.GetRefundByIdAsync(id);
            if (result == null) return NotFound();
            return Ok(result);
        }
    }
}