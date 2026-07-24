using ECommerce.Application.DTOs.Orders;
using FluentValidation;

namespace ECommerce.Application.Validators
{
    public class CreateShipmentDtoValidator : AbstractValidator<CreateShipmentDto>
    {
        public CreateShipmentDtoValidator()
        {
            RuleFor(x => x.SellerOrderId).NotEmpty();
        }
    }
}