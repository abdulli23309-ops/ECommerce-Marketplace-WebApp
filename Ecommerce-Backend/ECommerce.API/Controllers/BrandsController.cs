using ECommerce.Application.DTOs.Catalog.Brands;
using ECommerce.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    //[Authorize(Roles = "SuperAdmin")] // Only admins can manage brands for now
    [Authorize]
    public class BrandsController : ControllerBase
    {
        private readonly IBrandService _brandService;
        public BrandsController(IBrandService brandService) => _brandService = brandService;

        [HttpGet]
        [AllowAnonymous] // Everyone can view brands
        public async Task<IActionResult> GetAll() => Ok(await _brandService.GetAllAsync());

        [HttpPost]
        public async Task<IActionResult> Create(CreateBrandDto dto)
            => Ok(await _brandService.CreateAsync(dto));

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, CreateBrandDto dto)
        {
            var result = await _brandService.UpdateAsync(id, dto);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _brandService.DeleteAsync(id);
            return NoContent();
        }
    }
}