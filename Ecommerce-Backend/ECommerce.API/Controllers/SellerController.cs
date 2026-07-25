using ECommerce.Application.DTOs.Seller;
using ECommerce.Application.Interfaces;
using ECommerce.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ECommerce.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class SellerController : ControllerBase
    {
        private readonly ISellerService _sellerService;
        private readonly IOrderService _orderService;

        public SellerController(ISellerService sellerService, IOrderService orderService)
        {
            _sellerService = sellerService;
            _orderService = orderService;
        }

        private Guid GetUserId() => Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        [HttpPost("profile")]
        public async Task<IActionResult> CreateProfile(CreateSellerProfileDto dto)
        {
            var profile = await _sellerService.CreateProfileAsync(GetUserId(), dto);
            return Ok(profile);
        }

        [HttpGet("profile")]
        public async Task<IActionResult> GetProfile()
        {
            var profile = await _sellerService.GetProfileAsync(GetUserId());
            if (profile == null) return NotFound();
            return Ok(profile);
        }

        [HttpPost("store")]
        public async Task<IActionResult> CreateStore(CreateStoreDto dto)
        {
            var store = await _sellerService.CreateStoreAsync(GetUserId(), dto);
            return Ok(store);
        }

        [HttpGet("store")]
        public async Task<IActionResult> GetStore()
        {
            var store = await _sellerService.GetStoreAsync(GetUserId());
            if (store == null) return NotFound();
            return Ok(store);
        }
        [HttpGet("orders")]
        [Authorize(Roles = "Seller")]
        public async Task<IActionResult> GetSellerOrders()
        {
            var userId = GetUserId();
            var orders = await _orderService.GetSellerOrdersAsync(userId);
            return Ok(orders);
        }
        [HttpGet("status")]
        [Authorize]
        public async Task<IActionResult> GetSellerStatus()
        {
            var userId = GetUserId();
            var profile = await _sellerService.GetProfileAsync(userId);
            if (profile == null) return Ok(new { hasProfile = false, status = (string?)null });
            return Ok(new { hasProfile = true, status = profile.Status });
        }
    }
}