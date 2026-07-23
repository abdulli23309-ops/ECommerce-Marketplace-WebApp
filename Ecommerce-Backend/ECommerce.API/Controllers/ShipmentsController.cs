using ECommerce.Application.DTOs.Orders;
using ECommerce.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // sellers and admins only in real app, but we'll allow any authenticated for testing
    public class ShipmentsController : ControllerBase
    {
        private readonly IShipmentService _shipmentService;
        public ShipmentsController(IShipmentService shipmentService) => _shipmentService = shipmentService;

        [HttpPost]
        public async Task<IActionResult> Create(CreateShipmentDto dto)
            => Ok(await _shipmentService.CreateShipmentAsync(dto));

        [HttpPut("{shipmentId}/status")]
        public async Task<IActionResult> UpdateStatus(Guid shipmentId, UpdateShipmentStatusDto dto)
            => Ok(await _shipmentService.UpdateShipmentStatusAsync(shipmentId, dto));

        [HttpGet("order/{sellerOrderId}")]
        public async Task<IActionResult> GetByOrder(Guid sellerOrderId)
        {
            var shipment = await _shipmentService.GetShipmentByOrderAsync(sellerOrderId);
            if (shipment == null) return NotFound();
            return Ok(shipment);
        }
    }
}