using ECommerce.Application.DTOs.Product;
using ECommerce.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ECommerce.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        public ProductsController(IProductService productService) => _productService = productService;

        private Guid GetUserId() => Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        [HttpGet]
        public async Task<IActionResult> GetMyProducts()
            => Ok(await _productService.GetStoreProductsAsync(GetUserId()));

        [HttpPost]
        public async Task<IActionResult> CreateProduct(CreateProductDto dto)
            => Ok(await _productService.CreateProductAsync(GetUserId(), dto));
        [HttpGet("all")]
        [AllowAnonymous]
        public async Task<IActionResult> GetPagedProducts(
    [FromQuery] int page = 1,
    [FromQuery] int pageSize = 10)
        {
            var result = await _productService.GetPagedProductsAsync(page, pageSize);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(Guid id, UpdateProductDto dto)
        {
            var result = await _productService.UpdateProductAsync(id, dto);
            if (result == null) return NotFound();
            return Ok(result);
        }
    }

}