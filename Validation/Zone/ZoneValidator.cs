using FluentValidation;
using Logex.API.Dtos.ZoneDtos;

namespace Logex.API.Validation.Zone
{
    public class CreateZoneValidator : AbstractValidator<ZoneDto>
    {
        public CreateZoneValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Zone Name is required.")
                .MaximumLength(50)
                .WithMessage("Zone Name cannot exceed 50 characters.")
                .Matches(@"^[a-zA-Z0-9\s]+$")
                .WithMessage("Zone Name should only contain letters and numbers.");
        }
    }
}
