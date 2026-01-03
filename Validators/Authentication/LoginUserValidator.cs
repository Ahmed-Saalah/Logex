using FluentValidation;
using Logex.API.Dtos.IdentityDtos;

namespace Logex.API.Validators.Authentication
{
    public class LoginUserValidator : AbstractValidator<LoginDto>
    {
        public LoginUserValidator()
        {
            RuleFor(c => c.Email)
                .NotEmpty()
                .WithMessage("Enail is required")
                .EmailAddress()
                .WithMessage("Invalid email format");

            RuleFor(c => c.Password).NotEmpty().WithMessage("Password is required");
        }
    }
}
