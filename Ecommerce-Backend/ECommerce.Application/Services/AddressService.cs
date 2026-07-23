using ECommerce.Application.DTOs.Address;
using ECommerce.Application.Interfaces;
using ECommerce.Domain.Entities;

namespace ECommerce.Application.Services
{
    public class AddressService : IAddressService
    {
        private readonly IAddressRepository _addressRepo;
        public AddressService(IAddressRepository addressRepo) => _addressRepo = addressRepo;

        public async Task<IEnumerable<AddressDto>> GetUserAddressesAsync(Guid userId)
        {
            var addresses = await _addressRepo.GetByUserIdAsync(userId);
            return addresses.Select(a => new AddressDto
            {
                Id = a.Id,
                FullName = a.FullName,
                PhoneNumber = a.PhoneNumber,
                AddressLine1 = a.AddressLine1,
                AddressLine2 = a.AddressLine2,
                City = a.City,
                State = a.State,
                PostalCode = a.PostalCode,
                IsDefault = a.IsDefault
            });
        }

        public async Task<AddressDto> AddAddressAsync(Guid userId, CreateAddressDto dto)
        {
            var address = new Address
            {
                UserId = userId,
                FullName = dto.FullName,
                PhoneNumber = dto.PhoneNumber,
                AddressLine1 = dto.AddressLine1,
                AddressLine2 = dto.AddressLine2,
                City = dto.City,
                State = dto.State,
                PostalCode = dto.PostalCode,
                IsDefault = dto.IsDefault,
                CreatedAt = DateTime.UtcNow
            };

            // If this address is set as default, unset any previous default
            if (dto.IsDefault)
            {
                var existingAddresses = await _addressRepo.GetByUserIdAsync(userId);
                foreach (var addr in existingAddresses.Where(a => a.IsDefault))
                {
                    addr.IsDefault = false;
                    _addressRepo.Update(addr);
                }
            }

            await _addressRepo.AddAsync(address);
            await _addressRepo.SaveChangesAsync();

            return new AddressDto
            {
                Id = address.Id,
                FullName = address.FullName,
                PhoneNumber = address.PhoneNumber,
                AddressLine1 = address.AddressLine1,
                AddressLine2 = address.AddressLine2,
                City = address.City,
                State = address.State,
                PostalCode = address.PostalCode,
                IsDefault = address.IsDefault
            };
        }

        public async Task DeleteAddressAsync(Guid userId, Guid addressId)
        {
            var address = await _addressRepo.GetByIdAsync(addressId);
            if (address != null && address.UserId == userId)
            {
                _addressRepo.Delete(address);
                await _addressRepo.SaveChangesAsync();
            }
        }
    }
}