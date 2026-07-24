using ECommerce.Application.DTOs.Seller;
using FluentValidation;

namespace ECommerce.Application.Validators
{
    public class CreateSellerProfileDtoValidator : AbstractValidator<CreateSellerProfileDto>
    {
        public CreateSellerProfileDtoValidator()
        {
            RuleFor(x => x.BusinessName).NotEmpty().MaximumLength(200);
        }
    }
}