using ECommerce.Application.DTOs.Product;
using FluentValidation;

namespace ECommerce.Application.Validators
{
    public class CreateProductDtoValidator : AbstractValidator<CreateProductDto>
    {
        public CreateProductDtoValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(300);
            RuleFor(x => x.BasePrice).GreaterThan(0);
        }
    }
}