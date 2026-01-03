using FluentValidation;
using Logex.API.Dtos.ZoneRateDtos;

namespace Logex.API.Validation.ZoneRate
{
    public class UpdateZoneRateValidator : AbstractValidator<UpdateZoneRateDto>
    {
        public UpdateZoneRateValidator()
        {
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
