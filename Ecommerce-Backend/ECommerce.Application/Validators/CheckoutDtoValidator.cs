using ECommerce.Application.DTOs.Order;
using FluentValidation;

namespace ECommerce.Application.Validators
{
    public class CheckoutDtoValidator : AbstractValidator<CheckoutDto>
    {
        public CheckoutDtoValidator()
        {
            RuleFor(x => x.AddressId).NotEmpty();
        }
    }
}