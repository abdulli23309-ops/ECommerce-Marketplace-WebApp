using ECommerce.Application.Interfaces;
using ECommerce.Domain.Entities;
using ECommerce.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Infrastructure.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly ECommerceDbContext _context;
        public CartRepository(ECommerceDbContext context) => _context = context;

        public async Task<Cart?> GetCartByUserIdAsync(Guid userId)
            => await _context.Carts
                .Include(c => c.CartItems)
                .ThenInclude(ci => ci.Product)
                .FirstOrDefaultAsync(c => c.UserId == userId);

        public async Task AddCartAsync(Cart cart)
            => await _context.Carts.AddAsync(cart);

        public async Task AddCartItemAsync(CartItem item)
            => await _context.CartItems.AddAsync(item);

        public void UpdateCartItem(CartItem item)
            => _context.CartItems.Update(item);

        public void RemoveCartItem(CartItem item)
            => _context.CartItems.Remove(item);

        public async Task SaveChangesAsync() => await _context.SaveChangesAsync();

        public async Task<CartItem?> GetCartItemByIdAsync(Guid cartItemId)
    => await _context.CartItems.FindAsync(cartItemId);

        public void ClearCart(Cart cart)
            => _context.CartItems.RemoveRange(cart.CartItems);
    }
}