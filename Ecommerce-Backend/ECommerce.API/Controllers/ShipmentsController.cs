using ECommerce.Application.DTOs.Orders;
using ECommerce.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ECommerce.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // reads open to any authenticated user; writes locked down per-action below
    public class ShipmentsController : ControllerBase
    {
        private readonly IShipmentService _shipmentService;
        public ShipmentsController(IShipmentService shipmentService) => _shipmentService = shipmentService;

        private Guid GetUserId() => Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        private bool IsAdmin() => User.IsInRole("SuperAdmin");

        [HttpPost]
        [Authorize(Roles = "Seller,SuperAdmin")]
        public async Task<IActionResult> Create(CreateShipmentDto dto)
        {
            try
            {
                var result = await _shipmentService.CreateShipmentAsync(dto, GetUserId(), IsAdmin());
                return Ok(result);
            }
            catch (UnauthorizedAccessException ex)
            {
                // NOTE: ControllerBase.Forbid() does not accept a free-text message
                // (its string[] overload is for auth scheme names, not messages),
                // so a 403 with an explanatory body is returned manually instead.
                return StatusCode(StatusCodes.Status403Forbidden, new { message = ex.Message });
            }
        }

        [HttpPut("{shipmentId}/status")]
        [Authorize(Roles = "Seller,SuperAdmin")]
        public async Task<IActionResult> UpdateStatus(Guid shipmentId, UpdateShipmentStatusDto dto)
        {
            try
            {
                var result = await _shipmentService.UpdateShipmentStatusAsync(shipmentId, dto, GetUserId(), IsAdmin());
                return Ok(result);
            }
            catch (UnauthorizedAccessException ex)
            {
                return StatusCode(StatusCodes.Status403Forbidden, new { message = ex.Message });
            }
        }

        [HttpGet("order/{sellerOrderId}")]
        public async Task<IActionResult> GetByOrder(Guid sellerOrderId)
        {
            var shipment = await _shipmentService.GetShipmentByOrderAsync(sellerOrderId);
            if (shipment == null) return NotFound();
            return Ok(shipment);
        }
    }
}