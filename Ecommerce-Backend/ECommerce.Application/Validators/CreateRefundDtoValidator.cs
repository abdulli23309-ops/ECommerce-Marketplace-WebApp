using ECommerce.Application.DTOs.Refunds;
using FluentValidation;

namespace ECommerce.Application.Validators
{
    public class CreateRefundDtoValidator : AbstractValidator<CreateRefundDto>
    {
        public CreateRefundDtoValidator()
        {
            RuleFor(x => x.PaymentId).NotEmpty();
            RuleFor(x => x.ReturnRequestId).NotEmpty();
            RuleFor(x => x.Amount).GreaterThan(0);
        }
    }
}