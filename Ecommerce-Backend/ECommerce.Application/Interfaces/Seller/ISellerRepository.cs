using ECommerce.Domain.Entities;

namespace ECommerce.Application.Interfaces
{
    public interface ISellerRepository
    {
        Task<SellerProfile?> GetByUserIdAsync(Guid userId);
        Task AddProfileAsync(SellerProfile profile);
        void UpdateProfile(SellerProfile profile);
        Task<Store?> GetStoreBySellerIdAsync(Guid sellerProfileId);
        Task AddStoreAsync(Store store);
        void UpdateStore(Store store);
        Task<IEnumerable<SellerProfile>> GetAllAsync();
        Task<SellerProfile?> GetByIdAsync(Guid id);
        Task<int> GetSellerCountAsync();
        Task<int> GetPendingSellerCountAsync();
        Task SaveChangesAsync();
    }
}