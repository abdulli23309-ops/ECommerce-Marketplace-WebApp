using ECommerce.Application.DTOs.Catalog;
using ECommerce.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    // Class-level: locked down by default. Reads are explicitly opened back up
    // with [AllowAnonymous] below; writes stay SuperAdmin-only.
    [Authorize(Roles = "SuperAdmin")]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _service;
        public CategoriesController(ICategoryService service) => _service = service;

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll()
            => Ok(await _service.GetAllCategoriesAsync());

        [HttpPost]
        public async Task<IActionResult> Create(CreateCategoryDto dto)
            => Ok(await _service.CreateCategoryAsync(dto));

        [HttpGet("{categoryId}/subcategories")]
        [AllowAnonymous]
        public async Task<IActionResult> GetSubCategories(Guid categoryId)
            => Ok(await _service.GetSubCategoriesAsync(categoryId));

        [HttpPost("{categoryId}/subcategories")]
        public async Task<IActionResult> CreateSubCategory(Guid categoryId, CreateSubCategoryDto dto)
            => Ok(await _service.CreateSubCategoryAsync(categoryId, dto));
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(Guid id, CreateCategoryDto dto)
        {
            var result = await _service.UpdateCategoryAsync(id, dto);
            if (result == null) return NotFound();
            return Ok(result);
        }
        [HttpPut("{categoryId}/subcategories/{subCategoryId}")]
        public async Task<IActionResult> UpdateSubCategory(Guid categoryId, Guid subCategoryId, CreateSubCategoryDto dto)
        {
            var result = await _service.UpdateSubCategoryAsync(categoryId, subCategoryId, dto);
            if (result == null) return NotFound();
            return Ok(result);
        }
        [HttpDelete("{categoryId}/subcategories/{subCategoryId}")]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> DeleteSubCategory(Guid categoryId, Guid subCategoryId)
        {
            var result = await _service.DeleteSubCategoryAsync(categoryId, subCategoryId);
            if (!result) return NotFound();
            return NoContent();
        }
        [HttpDelete("{id}")]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> DeleteCategory(Guid id)
        {
            var category = await _service.GetByIdAsync(id);
            if (category == null) return NotFound();

            // Soft-delete the category (set IsDeleted = true)
            await _service.DeleteCategoryAsync(id);
            return NoContent();
        }
    }
}