using ECommerce.Application.DTOs.Seller;
using ECommerce.Application.Interfaces;
using ECommerce.Application.Services;
using Microsoft.AspNetCore.Authorization;
using ECommerce.Infrastructure.Services;
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
        private readonly IFileStorageService _fileStorageService;

        public SellerController(ISellerService sellerService, IOrderService orderService, IFileStorageService fileStorageService)
        {
            _sellerService = sellerService;
            _orderService = orderService;
            _fileStorageService = fileStorageService;
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
            if (profile == null) return Ok(new { hasProfile = false, status = (string?)null, rejectionReason = (string?)null });
            return Ok(new { hasProfile = true, status = profile.Status, rejectionReason = profile.RejectionReason });
        }
        [HttpPut("store")]
        [Authorize(Roles = "Seller")] // only a seller can update their own store
        public async Task<IActionResult> UpdateStore(UpdateStoreDto dto)
        {
            var userId = GetUserId();
            var result = await _sellerService.UpdateStoreAsync(userId, dto);
            if (result == null) return NotFound();
            return Ok(result);
        }
        /// <summary>
        /// Uploads a store logo image and returns the public URL.
        /// </summary>
        [HttpPost("store/logo")]
        [Authorize] // any authenticated user who is becoming a seller can upload a logo
        public async Task<IActionResult> UploadStoreLogo(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest(new { message = "No file provided." });

            try
            {
                var logoUrl = await _fileStorageService.SaveFileAsync(file.OpenReadStream(), file.FileName, "stores/logos");
                return Ok(new { logoUrl });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        [HttpGet("dashboard")]
        [Authorize(Roles = "Seller")]
        public async Task<IActionResult> GetDashboard()
        {
            var userId = GetUserId();
            var dashboard = await _sellerService.GetDashboardAsync(userId);
            return Ok(dashboard);
        }
        [HttpGet("reviews")]
        [Authorize(Roles = "Seller")]
        public async Task<IActionResult> GetStoreReviews([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var userId = GetUserId();
            var result = await _sellerService.GetStoreReviewsAsync(userId, page, pageSize);
            return Ok(result);
        }
    }
}