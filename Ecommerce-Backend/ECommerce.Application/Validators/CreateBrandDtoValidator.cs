using ECommerce.Application.DTOs.Catalog.Brands;
using FluentValidation;

namespace ECommerce.Application.Validators
{
    public class CreateBrandDtoValidator : AbstractValidator<CreateBrandDto>
    {
        public CreateBrandDtoValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
        }
    }
}