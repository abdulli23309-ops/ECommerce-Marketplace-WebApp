using ECommerce.Application.DTOs.Orders;
using FluentValidation;

namespace ECommerce.Application.Validators
{
    public class UpdateShipmentStatusDtoValidator : AbstractValidator<UpdateShipmentStatusDto>
    {
        public UpdateShipmentStatusDtoValidator()
        {
            RuleFor(x => x.Status).NotEmpty().MaximumLength(50);
        }
    }
}