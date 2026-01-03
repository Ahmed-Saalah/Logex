using FluentValidation;
using Logex.API.Dtos.ZoneRateDtos;

namespace Logex.API.Validation.ZoneRate
{
    public class CreateZoneRateValidator : AbstractValidator<CreateZoneRateDto>
    {
        public CreateZoneRateValidator()
        {
            RuleFor(x => x.FromZoneId).GreaterThan(0).WithMessage("FromZoneId is required.");

            RuleFor(x => x.ToZoneId).GreaterThan(0).WithMessage("ToZoneId is required.");

            RuleFor(x => x.BaseRate)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Base Rate cannot be negative.")
                .PrecisionScale(18, 2, false)
                .WithMessage("Invalid currency format.");

            When(
                x => x.AdditionalWeightCost.HasValue,
                () =>
                {
                    RuleFor(x => x.AdditionalWeightCost.Value)
                        .GreaterThanOrEqualTo(0)
                        .WithMessage("Additional Weight Cost cannot be negative.");
                }
            );
        }
    }
}
