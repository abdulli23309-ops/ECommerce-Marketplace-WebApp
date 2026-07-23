using ECommerce.Application.DTOs.Cart;
using ECommerce.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ECommerce.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;
        public CartController(ICartService cartService) => _cartService = cartService;

        private Guid GetUserId() => Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        [HttpGet]
        public async Task<IActionResult> GetCart()
            => Ok(await _cartService.GetCartAsync(GetUserId()));

        [HttpPost("add")]
        public async Task<IActionResult> AddToCart(AddToCartDto dto)
            => Ok(await _cartService.AddToCartAsync(GetUserId(), dto));

        [HttpDelete("remove/{cartItemId}")]
        public async Task<IActionResult> RemoveFromCart(Guid cartItemId)
        {
            await _cartService.RemoveFromCartAsync(GetUserId(), cartItemId);
            return NoContent();
        }
        [HttpPut("items/{cartItemId}")]
        public async Task<IActionResult> UpdateQuantity(Guid cartItemId, [FromBody] int quantity)
    => Ok(await _cartService.UpdateCartItemQuantityAsync(GetUserId(), cartItemId, quantity));

        [HttpPost("clear")]
        public async Task<IActionResult> ClearCart()
        {
            await _cartService.ClearCartAsync(GetUserId());
            return NoContent();
        }
    }
}