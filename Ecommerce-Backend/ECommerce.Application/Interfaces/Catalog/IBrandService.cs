using ECommerce.Application.DTOs.Catalog.Brands;

namespace ECommerce.Application.Interfaces
{
    public interface IBrandService
    {
        Task<IEnumerable<BrandDto>> GetAllAsync();
        Task<BrandDto> CreateAsync(CreateBrandDto dto);
        Task<BrandDto?> UpdateAsync(Guid id, CreateBrandDto dto);
        Task DeleteAsync(Guid id);
    }
}