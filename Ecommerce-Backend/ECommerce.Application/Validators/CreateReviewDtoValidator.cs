using ECommerce.Application.DTOs.Reviews;
using FluentValidation;

namespace ECommerce.Application.Validators
{
    public class CreateReviewDtoValidator : AbstractValidator<CreateReviewDto>
    {
        public CreateReviewDtoValidator()
        {
            RuleFor(x => x.OrderItemId).NotEmpty();
            RuleFor(x => x.Rating).InclusiveBetween(1, 5);
        }
    }
}