using ECommerce.Application.DTOs.Catalog;
using FluentValidation;

namespace ECommerce.Application.Validators
{
    public class CreateCategoryDtoValidator : AbstractValidator<CreateCategoryDto>
    {
        public CreateCategoryDtoValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
        }
    }
}