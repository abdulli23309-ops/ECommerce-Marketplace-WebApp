using ECommerce.Application.Interfaces;
using ECommerce.Domain.Entities;
using ECommerce.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Infrastructure.Repositories
{
    public class SellerRepository : ISellerRepository
    {
        private readonly ECommerceDbContext _context;
        public SellerRepository(ECommerceDbContext context) => _context = context;

        public async Task<SellerProfile?> GetByUserIdAsync(Guid userId)
            => await _context.SellerProfiles.FirstOrDefaultAsync(sp => sp.UserId == userId);

        public async Task AddProfileAsync(SellerProfile profile)
            => await _context.SellerProfiles.AddAsync(profile);

        public void UpdateProfile(SellerProfile profile)
            => _context.SellerProfiles.Update(profile);

        public async Task<Store?> GetStoreBySellerIdAsync(Guid sellerProfileId)
            => await _context.Stores.FirstOrDefaultAsync(s => s.SellerProfileId == sellerProfileId && !s.IsDeleted);

        public async Task AddStoreAsync(Store store)
            => await _context.Stores.AddAsync(store);

        public void UpdateStore(Store store)
            => _context.Stores.Update(store);
        public async Task<IEnumerable<SellerProfile>> GetAllAsync()
             => await _context.SellerProfiles
                 .Include(sp => sp.User)
                   .ToListAsync();
        public async Task<SellerProfile?> GetByIdAsync(Guid id)
    => await _context.SellerProfiles.FindAsync(id);
        public async Task SaveChangesAsync() => await _context.SaveChangesAsync();
        public async Task<int> GetSellerCountAsync()
    => await _context.SellerProfiles.CountAsync();

        public async Task<int> GetPendingSellerCountAsync()
            => await _context.SellerProfiles.CountAsync(sp => sp.Status == "Pending");
    }
}