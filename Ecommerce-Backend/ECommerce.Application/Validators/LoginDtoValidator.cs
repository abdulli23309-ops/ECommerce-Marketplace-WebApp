using ECommerce.Application.DTOs.Account;
using FluentValidation;

namespace ECommerce.Application.Validators
{
    public class LoginDtoValidator : AbstractValidator<LoginDto>
    {
        public LoginDtoValidator()
        {
            RuleFor(x => x.Email).NotEmpty().EmailAddress().MaximumLength(256);
            RuleFor(x => x.Password).NotEmpty().MinimumLength(6).MaximumLength(100);
        }
    }
}