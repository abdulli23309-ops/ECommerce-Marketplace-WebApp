using ECommerce.Application.DTOs.Catalog;

namespace ECommerce.Application.Interfaces
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync();
        Task<CategoryDto> CreateCategoryAsync(CreateCategoryDto dto);
        Task<IEnumerable<SubCategoryDto>> GetSubCategoriesAsync(Guid categoryId);
        Task<SubCategoryDto> CreateSubCategoryAsync(Guid categoryId, CreateSubCategoryDto dto);
        Task<CategoryDto?> UpdateCategoryAsync(Guid id, CreateCategoryDto dto);
        Task<bool> DeleteSubCategoryAsync(Guid categoryId, Guid subCategoryId);
        Task<CategoryDto?> GetByIdAsync(Guid id);
        Task DeleteCategoryAsync(Guid id);
        Task<SubCategoryDto?> UpdateSubCategoryAsync(Guid categoryId, Guid subCategoryId, CreateSubCategoryDto dto);
    }
}