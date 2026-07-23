using ECommerce.Application.Interfaces;
using ECommerce.Domain.Entities;
using ECommerce.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Infrastructure.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ECommerceDbContext _context;
        public CategoryRepository(ECommerceDbContext context) => _context = context;

        public async Task<IEnumerable<Category>> GetAllAsync()
            => await _context.Categories.Where(c => !c.IsDeleted).ToListAsync();

        public async Task<Category?> GetByIdAsync(Guid id)
            => await _context.Categories.FirstOrDefaultAsync(c => c.Id == id && !c.IsDeleted);

        public async Task AddAsync(Category category)
            => await _context.Categories.AddAsync(category);

        public void Update(Category category)
            => _context.Categories.Update(category);

        public async Task<IEnumerable<SubCategory>> GetSubCategoriesAsync(Guid categoryId)
            => await _context.SubCategories.Where(sc => sc.CategoryId == categoryId && !sc.IsDeleted).ToListAsync();

        public async Task AddSubCategoryAsync(SubCategory subCategory)
            => await _context.SubCategories.AddAsync(subCategory);

        public void UpdateSubCategory(SubCategory subCategory)
            => _context.SubCategories.Update(subCategory);

        public async Task SaveChangesAsync() => await _context.SaveChangesAsync();
    }
}