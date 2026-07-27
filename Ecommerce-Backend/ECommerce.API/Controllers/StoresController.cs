using ECommerce.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StoresController : ControllerBase
    {
        private readonly ISellerService _sellerService;
        public StoresController(ISellerService sellerService)
        {
            _sellerService = sellerService;
        }

        [HttpGet("{storeId:guid}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetStoreDetails(Guid storeId)
        {
            var store = await _sellerService.GetStorePublicInfoAsync(storeId);
            if (store == null) return NotFound();
            return Ok(store);
        }
    }
}