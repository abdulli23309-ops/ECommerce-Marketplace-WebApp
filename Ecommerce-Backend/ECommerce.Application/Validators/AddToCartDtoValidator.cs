using ECommerce.Application.DTOs.Cart;
using FluentValidation;

namespace ECommerce.Application.Validators
{
    public class AddToCartDtoValidator : AbstractValidator<AddToCartDto>
    {
        public AddToCartDtoValidator()
        {
            RuleFor(x => x.ProductId).NotEmpty();
            RuleFor(x => x.Quantity).GreaterThan(0);
        }
    }
}