using FluentValidation;
using Logex.API.Models;

namespace Logex.API.Validators.Pricing
{
    public class PricingValidator : AbstractValidator<Shipment>
    {
        public PricingValidator()
        {
            RuleFor(_ => _.ShipmentMethod)
                .NotNull()
                .WithMessage("ShipmentMethod is required for price calculation.");

            RuleFor(_ => _.ShipperCity)
                .NotEmpty()
                .WithMessage("ShipperCity is required for price calculation.");

            RuleFor(_ => _.ReceiverCity)
                .NotEmpty()
                .WithMessage("ReceiverCity is required for price calculation.");

            RuleFor(_ => _.ShipperCountry)
                .NotEmpty()
                .WithMessage("ShipperCountry is required for price calculation.");

            RuleFor(_ => _.ReceiverCountry)
                .NotEmpty()
                .WithMessage("ReceiverCountry is required for price calculation.");

            RuleFor(_ => _.ShipperStreet)
                .NotEmpty()
                .WithMessage("ShipperStreet is required for price calculation.");

            RuleFor(_ => _.ReceiverStreet)
                .NotEmpty()
                .WithMessage("ReceiverStreet is required for price calculation.");

            RuleFor(_ => _.ReceiverName)
                .NotEmpty()
                .WithMessage("ReceiverName is required for price calculation.");
            RuleFor(_ => _.ShipperName)
                .NotEmpty()
                .WithMessage("ShipperName is required for price calculation.");

            RuleFor(_ => _.ReceiverPhone)
                .NotEmpty()
                .WithMessage("ReceiverPhone is required for price calculation.");

            RuleFor(_ => _.ShipperPhone)
                .NotEmpty()
                .WithMessage("ShipperPhone is required for price calculation.");

            RuleFor(_ => _.Quantity)
                .GreaterThan(0)
                .WithMessage("Quantity must be greater than zero for price calculation.");

            RuleFor(_ => _.Weight)
                .GreaterThan(0)
                .WithMessage("Weight must be greater than zero for price calculation.");
        }
    }
}
