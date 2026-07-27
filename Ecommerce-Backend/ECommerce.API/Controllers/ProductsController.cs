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

        // Seller's own products
        [HttpGet]
        [Authorize(Roles = "Seller")]
        public async Task<IActionResult> GetMyProducts()
            => Ok(await _productService.GetStoreProductsAsync(GetUserId()));

        // Create product (Seller only)
        [HttpPost]
        [Authorize(Roles = "Seller")]
        public async Task<IActionResult> CreateProduct(CreateProductDto dto)
            => Ok(await _productService.CreateProductAsync(GetUserId(), dto));

        // Public: paginated listing of approved products
        [HttpGet("all")]
        [AllowAnonymous]
        public async Task<IActionResult> GetPagedProducts(
    [FromQuery] int page = 1,
    [FromQuery] int pageSize = 10,
    [FromQuery] Guid? categoryId = null,
    [FromQuery] Guid? subCategoryId = null,
    [FromQuery] Guid? brandId = null,
    [FromQuery] decimal? minPrice = null,
    [FromQuery] decimal? maxPrice = null,
    [FromQuery] string? search = null,
    [FromQuery] string? sortBy = null)
        {
            var result = await _productService.GetPagedProductsAsync(
                page, pageSize, categoryId, subCategoryId, brandId,
                minPrice, maxPrice, search, sortBy);
            return Ok(result);
        }

        // Public: single product detail
        [HttpGet("{id:guid}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(Guid id)
        {
            var product = await _productService.GetProductDetailAsync(id);
            if (product == null) return NotFound();
            return Ok(product);
        }

        // Public: products by store
        [HttpGet("store/{storeId:guid}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetByStore(Guid storeId)
        {
            var products = await _productService.GetPublicStoreProductsAsync(storeId);
            return Ok(products);
        }

        // Update product (Seller only, with ownership check)
        [HttpPut("{id}")]
        [Authorize(Roles = "Seller")]
        public async Task<IActionResult> UpdateProduct(Guid id, UpdateProductDto dto)
        {
            try
            {
                var result = await _productService.UpdateProductAsync(GetUserId(), id, dto);
                if (result == null) return NotFound();
                return Ok(result);
            }
            catch (UnauthorizedAccessException ex)
            {
                return StatusCode(StatusCodes.Status403Forbidden, new { message = ex.Message });
            }
        }

        // Delete product (soft delete, Seller only with ownership check)
        [HttpDelete("{id}")]
        [Authorize(Roles = "Seller")]
        public async Task<IActionResult> DeleteProduct(Guid id)
        {
            try
            {
                var deleted = await _productService.DeleteProductAsync(GetUserId(), id);
                if (!deleted) return NotFound();
                return NoContent();
            }
            catch (UnauthorizedAccessException ex)
            {
                return StatusCode(StatusCodes.Status403Forbidden, new { message = ex.Message });
            }
        }
        [HttpPost("{id}/images")]
        [Authorize(Roles = "Seller")]
        public async Task<IActionResult> UploadImage(Guid id, IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file provided.");

            try
            {
                var result = await _productService.UploadProductImageAsync(GetUserId(), id, file.OpenReadStream(), file.FileName);
                return Ok(result);
            }
            catch (UnauthorizedAccessException ex)
            {
                return StatusCode(StatusCodes.Status403Forbidden, new { message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}/images/{imageId}")]
        [Authorize(Roles = "Seller")]
        public async Task<IActionResult> DeleteImage(Guid id, Guid imageId)
        {
            try
            {
                await _productService.DeleteProductImageAsync(GetUserId(), id, imageId);
                return NoContent();
            }
            catch (UnauthorizedAccessException ex)
            {
                return StatusCode(StatusCodes.Status403Forbidden, new { message = ex.Message });
            }
        }
    }
}