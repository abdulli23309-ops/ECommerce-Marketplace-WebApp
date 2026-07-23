using ECommerce.Domain.Entities;

namespace ECommerce.Application.Interfaces
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<Category>> GetAllAsync();
        Task<Category?> GetByIdAsync(Guid id);
        Task AddAsync(Category category);
        void Update(Category category);
        Task<IEnumerable<SubCategory>> GetSubCategoriesAsync(Guid categoryId);
        Task AddSubCategoryAsync(SubCategory subCategory);
        void UpdateSubCategory(SubCategory subCategory);
        Task SaveChangesAsync();
    }
}