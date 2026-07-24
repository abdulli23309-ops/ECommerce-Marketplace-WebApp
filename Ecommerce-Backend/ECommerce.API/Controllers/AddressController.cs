using ECommerce.Application.DTOs.Address;
using ECommerce.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ECommerce.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class AddressController : ControllerBase
    {
        private readonly IAddressService _addressService;
        public AddressController(IAddressService addressService) => _addressService = addressService;

        private Guid GetUserId() => Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        [HttpGet]
        public async Task<IActionResult> GetAddresses()
            => Ok(await _addressService.GetUserAddressesAsync(GetUserId()));

        [HttpPost]
        public async Task<IActionResult> AddAddress(CreateAddressDto dto)
            => Ok(await _addressService.AddAddressAsync(GetUserId(), dto));

        [HttpDelete("{addressId}")]
        public async Task<IActionResult> DeleteAddress(Guid addressId)
        {
            await _addressService.DeleteAddressAsync(GetUserId(), addressId);
            return NoContent();
        }
        [HttpPut("{id}/default")]
        [Authorize]
        public async Task<IActionResult> SetDefaultAddress(Guid id)
        {
            var success = await _addressService.SetDefaultAddressAsync(GetUserId(), id);
            if (!success) return NotFound();
            return NoContent();
        }
    }
}