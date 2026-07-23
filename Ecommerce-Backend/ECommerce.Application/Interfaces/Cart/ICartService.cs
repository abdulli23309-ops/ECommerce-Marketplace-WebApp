using ECommerce.Application.DTOs.Cart;

namespace ECommerce.Application.Interfaces
{
    public interface ICartService
    {
        Task<CartDto> GetCartAsync(Guid userId);
        Task<CartDto> AddToCartAsync(Guid userId, AddToCartDto dto);
        Task RemoveFromCartAsync(Guid userId, Guid cartItemId);
        Task<CartDto> UpdateCartItemQuantityAsync(Guid userId, Guid cartItemId, int newQuantity);
        Task ClearCartAsync(Guid userId);
    }
}