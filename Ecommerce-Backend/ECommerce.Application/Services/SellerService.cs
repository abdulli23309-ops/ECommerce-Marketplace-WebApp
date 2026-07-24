using ECommerce.Application.DTOs.Seller;
using ECommerce.Application.Interfaces;
using ECommerce.Domain.Entities;

namespace ECommerce.Application.Services.Seller
{
    public class SellerService : ISellerService
    {
        private readonly ISellerRepository _repo;
        public SellerService(ISellerRepository repo) => _repo = repo;

        public async Task<SellerProfileDto?> GetProfileAsync(Guid userId)
        {
            var profile = await _repo.GetByUserIdAsync(userId);
            if (profile == null) return null;
            return new SellerProfileDto
            {
                Id = profile.Id,
                BusinessName = profile.BusinessName,
                Description = profile.Description,
                Status = profile.Status
            };
        }

        public async Task<SellerProfileDto> CreateProfileAsync(Guid userId, CreateSellerProfileDto dto)
        {
            var profile = new SellerProfile
            {
                UserId = userId,
                BusinessName = dto.BusinessName,
                Description = dto.Description,
                Status = "Pending",
                CreatedAt = DateTime.UtcNow
            };
            await _repo.AddProfileAsync(profile);
            await _repo.SaveChangesAsync();
            return new SellerProfileDto
            {
                Id = profile.Id,
                BusinessName = profile.BusinessName,
                Description = profile.Description,
                Status = profile.Status
            };
        }

        public async Task<StoreDto?> GetStoreAsync(Guid userId)
        {
            var profile = await _repo.GetByUserIdAsync(userId);
            if (profile == null) return null;
            var store = await _repo.GetStoreBySellerIdAsync(profile.Id);
            if (store == null) return null;
            return new StoreDto
            {
                Id = store.Id,
                Name = store.Name,
                Description = store.Description,
                IsActive = store.IsActive
            };
        }

        public async Task<StoreDto> CreateStoreAsync(Guid userId, CreateStoreDto dto)
        {
            var profile = await _repo.GetByUserIdAsync(userId);
            if (profile == null) throw new InvalidOperationException("Seller profile not found.");
            var store = new Store
            {
                SellerProfileId = profile.Id,
                Name = dto.Name,
                Description = dto.Description,
                CreatedAt = DateTime.UtcNow
            };
            await _repo.AddStoreAsync(store);
            await _repo.SaveChangesAsync();
            return new StoreDto
            {
                Id = store.Id,
                Name = store.Name,
                Description = store.Description,
                IsActive = store.IsActive
            };
        }
    }
}