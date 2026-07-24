using ECommerce.Application.DTOs.Catalog;
using FluentValidation;

namespace ECommerce.Application.Validators
{
    public class CreateSubCategoryDtoValidator : AbstractValidator<CreateSubCategoryDto>
    {
        public CreateSubCategoryDtoValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
        }
    }
}