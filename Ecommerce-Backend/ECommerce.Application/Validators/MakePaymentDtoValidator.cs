using ECommerce.Application.DTOs.Payment;
using FluentValidation;

namespace ECommerce.Application.Validators
{
    public class MakePaymentDtoValidator : AbstractValidator<MakePaymentDto>
    {
        public MakePaymentDtoValidator()
        {
            RuleFor(x => x.OrderId).NotEmpty();
            RuleFor(x => x.Method).NotEmpty().MaximumLength(50);
        }
    }
}