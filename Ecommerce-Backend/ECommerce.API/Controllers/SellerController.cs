using ECommerce.Application.DTOs.Seller;
using ECommerce.Application.Interfaces;
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

        public SellerController(ISellerService sellerService)
        {
            _sellerService = sellerService;
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
    }
}