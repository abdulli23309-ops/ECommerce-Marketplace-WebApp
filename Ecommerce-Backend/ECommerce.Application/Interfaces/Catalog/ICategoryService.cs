using ECommerce.Application.DTOs.Catalog;

namespace ECommerce.Application.Interfaces
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync();
        Task<CategoryDto> CreateCategoryAsync(CreateCategoryDto dto);
        Task<IEnumerable<SubCategoryDto>> GetSubCategoriesAsync(Guid categoryId);
        Task<SubCategoryDto> CreateSubCategoryAsync(Guid categoryId, CreateSubCategoryDto dto);
    }
}