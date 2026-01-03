using FluentValidation;
using Logex.API.Dtos.CityDtos;

namespace Logex.API.Validators.City
{
    public class CreateCityValidator : AbstractValidator<CreateCityDto>
    {
        public CreateCityValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("City Name is required.")
                .MaximumLength(50)
                .WithMessage("City Name cannot exceed 50 characters.");

            RuleFor(x => x.ZoneId).GreaterThan(0).WithMessage("A valid Zone ID is required.");
        }
    }
}
