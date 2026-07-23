using ECommerce.Domain.Entities;

namespace ECommerce.Application.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetByEmailAsync(string email);
        Task<User?> GetByIdAsync(Guid id);
        Task AddAsync(User user);
        void Update(User user);
        Task SaveChangesAsync();
        Task<User?> GetByRefreshTokenAsync(string refreshToken);
    }
}