using ECommerce.Domain.Entities;

namespace ECommerce.Application.Interfaces
{
    public interface ICartRepository
    {
        Task<Cart?> GetCartByUserIdAsync(Guid userId);
        Task AddCartAsync(Cart cart);
        Task AddCartItemAsync(CartItem item);
        void UpdateCartItem(CartItem item);
        void RemoveCartItem(CartItem item);
        Task SaveChangesAsync();
        Task<CartItem?> GetCartItemByIdAsync(Guid cartItemId);
        void ClearCart(Cart cart);
    }
}