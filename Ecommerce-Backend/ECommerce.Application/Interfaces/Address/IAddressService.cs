using ECommerce.Application.DTOs.Address;

namespace ECommerce.Application.Interfaces
{
    public interface IAddressService
    {
        Task<IEnumerable<AddressDto>> GetUserAddressesAsync(Guid userId);
        Task<AddressDto> AddAddressAsync(Guid userId, CreateAddressDto dto);
        Task<bool> SetDefaultAddressAsync(Guid userId, Guid addressId);
        Task DeleteAddressAsync(Guid userId, Guid addressId);
    }
}