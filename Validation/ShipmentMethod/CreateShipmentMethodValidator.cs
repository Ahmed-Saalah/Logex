using FluentValidation;
using Logex.API.Dtos.ShipmentMethodDtos;

namespace Logex.API.Validation.ShipmentMethod
{
    public class CreateShipmentMethodValidator : AbstractValidator<CreateShipmentMethodDto>
    {
        public CreateShipmentMethodValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Method name is required.")
                .MaximumLength(50)
                .WithMessage("Name cannot exceed 50 characters.");

            RuleFor(x => x.Duration)
                .NotEmpty()
                .WithMessage("Duration is required.")
                .MaximumLength(20)
                .WithMessage(errorMessage: "Duration cannot exceed 20 characters.");

            RuleFor(x => x.Cost)
                .GreaterThan(0)
                .WithMessage("Cost must be greater than zero.")
                .PrecisionScale(18, 2, false)
                .WithMessage("Invalid currency format.");
        }
    }
}
