using ECommerce.Application.DTOs.Seller;
using ECommerce.Application.Helpers;

namespace ECommerce.Application.Interfaces
{
    public interface ISellerService
    {
        Task<SellerProfileDto?> GetProfileAsync(Guid userId);
        Task<SellerProfileDto> CreateProfileAsync(Guid userId, CreateSellerProfileDto dto);
        Task<StoreDto?> GetStoreAsync(Guid userId);
        Task<StoreDto?> UpdateStoreAsync(Guid userId, UpdateStoreDto dto);
        Task<SellerDashboardDto> GetDashboardAsync(Guid userId);
        Task<StorePublicInfoDto?> GetStorePublicInfoAsync(Guid storeId);
        Task<PagedResult<SellerReviewDto>> GetStoreReviewsAsync(Guid userId, int page, int pageSize);
        Task<StoreDto> CreateStoreAsync(Guid userId, CreateStoreDto dto);
    }
}