using ECommerce.Application.DTOs.Cart;
using ECommerce.Application.Interfaces;
using ECommerce.Domain.Entities;

namespace ECommerce.Application.Services
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepo;
        private readonly IProductRepository _productRepo; // new dependency

        public CartService(ICartRepository cartRepo, IProductRepository productRepo)
        {
            _cartRepo = cartRepo;
            _productRepo = productRepo;
        }

        public async Task<CartDto> GetCartAsync(Guid userId)
        {
            var cart = await _cartRepo.GetCartByUserIdAsync(userId);
            if (cart == null) return new CartDto();

            return MapCartToDto(cart);
        }

        public async Task<CartDto> AddToCartAsync(Guid userId, AddToCartDto dto)
        {
            var product = await _productRepo.GetByIdAsync(dto.ProductId)
                          ?? throw new InvalidOperationException("Product not found.");

            if (product.IsDeleted || product.Status != "Approved")
                throw new InvalidOperationException("Product is not available.");

            if (dto.Quantity > product.StockQuantity)
                throw new InvalidOperationException("Insufficient stock.");

            var cart = await _cartRepo.GetCartByUserIdAsync(userId);
            if (cart == null)
            {
                cart = new Cart { UserId = userId, CreatedAt = DateTime.UtcNow };
                await _cartRepo.AddCartAsync(cart);
                await _cartRepo.SaveChangesAsync();
            }

            var existingItem = cart.CartItems.FirstOrDefault(ci => ci.ProductId == dto.ProductId);
            if (existingItem != null)
            {
                int newQuantity = existingItem.Quantity + dto.Quantity;
                if (newQuantity > product.StockQuantity)
                    throw new InvalidOperationException("Insufficient stock.");

                existingItem.Quantity = newQuantity;
                _cartRepo.UpdateCartItem(existingItem);
            }
            else
            {
                var item = new CartItem
                {
                    CartId = cart.Id,
                    ProductId = dto.ProductId,
                    Quantity = dto.Quantity,
                    AddedAt = DateTime.UtcNow
                };
                await _cartRepo.AddCartItemAsync(item);
            }

            await _cartRepo.SaveChangesAsync();
            return await GetCartAsync(userId);
        }

        public async Task<CartDto> UpdateCartItemQuantityAsync(Guid userId, Guid cartItemId, int newQuantity)
        {
            var cart = await _cartRepo.GetCartByUserIdAsync(userId)
                        ?? throw new InvalidOperationException("Cart not found.");

            var cartItem = cart.CartItems.FirstOrDefault(ci => ci.Id == cartItemId)
                           ?? throw new InvalidOperationException("Cart item not found.");

            var product = await _productRepo.GetByIdAsync(cartItem.ProductId)
                          ?? throw new InvalidOperationException("Product no longer exists.");

            if (newQuantity > product.StockQuantity)
                throw new InvalidOperationException("Insufficient stock.");

            cartItem.Quantity = newQuantity;
            _cartRepo.UpdateCartItem(cartItem);
            await _cartRepo.SaveChangesAsync();

            return MapCartToDto(cart);
        }

        public async Task RemoveFromCartAsync(Guid userId, Guid cartItemId)
        {
            var cart = await _cartRepo.GetCartByUserIdAsync(userId);
            var item = cart?.CartItems.FirstOrDefault(ci => ci.Id == cartItemId);
            if (item != null)
            {
                _cartRepo.RemoveCartItem(item);
                await _cartRepo.SaveChangesAsync();
            }
        }

        public async Task ClearCartAsync(Guid userId)
        {
            var cart = await _cartRepo.GetCartByUserIdAsync(userId);
            if (cart != null)
            {
                _cartRepo.ClearCart(cart);
                await _cartRepo.SaveChangesAsync();
            }
        }

        private CartDto MapCartToDto(Cart cart)
        {
            return new CartDto
            {
                CartId = cart.Id,
                Items = cart.CartItems.Select(ci => new CartItemDto
                {
                    CartItemId = ci.Id,
                    ProductId = ci.ProductId,
                    ProductName = ci.Product?.Name ?? "Deleted Product",
                    Quantity = ci.Quantity,
                    UnitPrice = ci.Product?.BasePrice ?? 0
                }).ToList()
            };
        }
    }
}