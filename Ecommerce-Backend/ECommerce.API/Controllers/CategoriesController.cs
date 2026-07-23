using ECommerce.Application.DTOs.Catalog;
using ECommerce.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _service;
        public CategoriesController(ICategoryService service) => _service = service;

        [HttpGet]
        public async Task<IActionResult> GetAll()
            => Ok(await _service.GetAllCategoriesAsync());

        [HttpPost]
        public async Task<IActionResult> Create(CreateCategoryDto dto)
            => Ok(await _service.CreateCategoryAsync(dto));

        [HttpGet("{categoryId}/subcategories")]
        public async Task<IActionResult> GetSubCategories(Guid categoryId)
            => Ok(await _service.GetSubCategoriesAsync(categoryId));

        [HttpPost("{categoryId}/subcategories")]
        public async Task<IActionResult> CreateSubCategory(Guid categoryId, CreateSubCategoryDto dto)
            => Ok(await _service.CreateSubCategoryAsync(categoryId, dto));
    }
}