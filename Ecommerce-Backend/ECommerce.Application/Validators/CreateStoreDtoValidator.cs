using ECommerce.Application.DTOs.Seller;
using FluentValidation;

namespace ECommerce.Application.Validators
{
    public class CreateStoreDtoValidator : AbstractValidator<CreateStoreDto>
    {
        public CreateStoreDtoValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
        }
    }
}