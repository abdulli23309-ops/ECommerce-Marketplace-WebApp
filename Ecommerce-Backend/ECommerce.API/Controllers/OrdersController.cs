using ECommerce.Application.DTOs.Order;
using ECommerce.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ECommerce.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;
        public OrdersController(IOrderService orderService) => _orderService = orderService;

        private Guid GetUserId() => Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        [HttpPost("checkout")]
        public async Task<IActionResult> Checkout(CheckoutDto dto)
            => Ok(await _orderService.CheckoutAsync(GetUserId(), dto));

        [HttpGet]
        public async Task<IActionResult> GetMyOrders()
            => Ok(await _orderService.GetMyOrdersAsync(GetUserId()));
    }
}