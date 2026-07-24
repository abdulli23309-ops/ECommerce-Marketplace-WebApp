using ECommerce.Application.DTOs.Refunds;
using ECommerce.Application.Interfaces;
using ECommerce.Application.Services.Refunds;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    // [Authorize(Roles = "SuperAdmin")] // admin only – for testing, you can temporarily change to [Authorize]
    [Authorize]
    public class RefundsController : ControllerBase
    {
        private readonly IRefundService _refundService;
        public RefundsController(IRefundService refundService) => _refundService = refundService;

        [HttpPost]
        public async Task<IActionResult> CreateRefund(CreateRefundDto dto)
        {
            var result = await _refundService.CreateRefundAsync(dto);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetRefund(Guid id)
        {
            var result = await _refundService.GetRefundByIdAsync(id);
            if (result == null) return NotFound();
            return Ok(result);
        }
    }
}