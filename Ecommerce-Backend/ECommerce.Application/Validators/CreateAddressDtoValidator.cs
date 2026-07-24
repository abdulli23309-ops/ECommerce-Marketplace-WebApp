using ECommerce.Application.DTOs.Address;
using FluentValidation;

namespace ECommerce.Application.Validators
{
    public class CreateAddressDtoValidator : AbstractValidator<CreateAddressDto>
    {
        public CreateAddressDtoValidator()
        {
            RuleFor(x => x.FullName).NotEmpty().MaximumLength(200);
            RuleFor(x => x.PhoneNumber).NotEmpty().MaximumLength(20);
            RuleFor(x => x.AddressLine1).NotEmpty().MaximumLength(300);
            RuleFor(x => x.City).NotEmpty().MaximumLength(100);
        }
    }
}