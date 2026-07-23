using ECommerce.Application.DTOs.Seller;

namespace ECommerce.Application.Interfaces
{
    public interface ISellerService
    {
        Task<SellerProfileDto?> GetProfileAsync(Guid userId);
        Task<SellerProfileDto> CreateProfileAsync(Guid userId, CreateSellerProfileDto dto);
        Task<StoreDto?> GetStoreAsync(Guid userId);
        Task<StoreDto> CreateStoreAsync(Guid userId, CreateStoreDto dto);
    }
}