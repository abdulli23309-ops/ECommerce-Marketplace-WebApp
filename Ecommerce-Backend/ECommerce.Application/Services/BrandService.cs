using ECommerce.Application.DTOs.Catalog.Brands;
using ECommerce.Application.Interfaces;
using ECommerce.Domain.Entities;

namespace ECommerce.Application.Services.Catalog
{
    public class BrandService : IBrandService
    {
        private readonly IBrandRepository _repo;
        public BrandService(IBrandRepository repo) => _repo = repo;

        public async Task<IEnumerable<BrandDto>> GetAllAsync()
        {
            var brands = await _repo.GetAllAsync();
            return brands.Select(b => new BrandDto { Id = b.Id, Name = b.Name });
        }

        public async Task<BrandDto> CreateAsync(CreateBrandDto dto)
        {
            var brand = new Brand { Name = dto.Name, CreatedAt = DateTime.UtcNow };
            await _repo.AddAsync(brand);
            await _repo.SaveChangesAsync();
            return new BrandDto { Id = brand.Id, Name = brand.Name };
        }

        public async Task<BrandDto?> UpdateAsync(Guid id, CreateBrandDto dto)
        {
            var brand = await _repo.GetByIdAsync(id);
            if (brand == null) return null;
            brand.Name = dto.Name;
            brand.UpdatedAt = DateTime.UtcNow;
            _repo.Update(brand);
            await _repo.SaveChangesAsync();
            return new BrandDto { Id = brand.Id, Name = brand.Name };
        }

        public async Task DeleteAsync(Guid id)
        {
            var brand = await _repo.GetByIdAsync(id);
            if (brand != null)
            {
                _repo.Delete(brand);
                await _repo.SaveChangesAsync();
            }
        }
    }
}