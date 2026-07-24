using ECommerce.Application.DTOs.Catalog;
using ECommerce.Application.Interfaces;
using ECommerce.Domain.Entities;

namespace ECommerce.Application.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _repo;
        public CategoryService(ICategoryRepository repo) => _repo = repo;

        public async Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync()
        {
            var categories = await _repo.GetAllAsync();
            return categories.Select(c => new CategoryDto { Id = c.Id, Name = c.Name });
        }

        public async Task<CategoryDto> CreateCategoryAsync(CreateCategoryDto dto)
        {
            var category = new Category { Name = dto.Name, CreatedAt = DateTime.UtcNow };
            await _repo.AddAsync(category);
            await _repo.SaveChangesAsync();
            return new CategoryDto { Id = category.Id, Name = category.Name };
        }

        public async Task<IEnumerable<SubCategoryDto>> GetSubCategoriesAsync(Guid categoryId)
        {
            var subs = await _repo.GetSubCategoriesAsync(categoryId);
            return subs.Select(s => new SubCategoryDto { Id = s.Id, Name = s.Name, CategoryId = s.CategoryId });
        }

        public async Task<SubCategoryDto> CreateSubCategoryAsync(Guid categoryId, CreateSubCategoryDto dto)
        {
            var sub = new SubCategory { CategoryId = categoryId, Name = dto.Name, CreatedAt = DateTime.UtcNow };
            await _repo.AddSubCategoryAsync(sub);
            await _repo.SaveChangesAsync();
            return new SubCategoryDto { Id = sub.Id, Name = sub.Name, CategoryId = sub.CategoryId };
        }
        public async Task<CategoryDto?> UpdateCategoryAsync(Guid id, CreateCategoryDto dto)
        {
            var category = await _repo.GetByIdAsync(id);
            if (category == null) return null;
            category.Name = dto.Name;
            category.UpdatedAt = DateTime.UtcNow;
            _repo.Update(category);
            await _repo.SaveChangesAsync();
            return new CategoryDto { Id = category.Id, Name = category.Name };
        }
        public async Task<SubCategoryDto?> UpdateSubCategoryAsync(Guid categoryId, Guid subCategoryId, CreateSubCategoryDto dto)
        {
            var sub = await _repo.GetSubCategoriesAsync(categoryId);
            var target = sub.FirstOrDefault(s => s.Id == subCategoryId);
            if (target == null) return null;
            target.Name = dto.Name;
            target.UpdatedAt = DateTime.UtcNow;
            _repo.UpdateSubCategory(target);
            await _repo.SaveChangesAsync();
            return new SubCategoryDto { Id = target.Id, Name = target.Name, CategoryId = target.CategoryId };
        }
    }
}