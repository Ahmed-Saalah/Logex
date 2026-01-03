using FluentValidation;
using Logex.API.Dtos.IdentityDtos;

namespace Logex.API.Validators.Authentication
{
    public class CreateUserValidator : AbstractValidator<RegisterDto>
    {
        public CreateUserValidator()
        {
            RuleFor(c => c.Username).NotEmpty().WithMessage("Full name is required");

            RuleFor(c => c.Email)
                .NotEmpty()
                .WithMessage("Email is required")
                .EmailAddress()
                .WithMessage("Invalid email");

            RuleFor(c => c.Password)
                .NotEmpty()
                .WithMessage("Password is required")
                .MinimumLength(8)
                .WithMessage("Password length must be at least 8")
                .Matches(@"[A-Z]")
                .WithMessage("Password must contain at leat one uppercase letter")
                .Matches(@"[a-z]")
                .WithMessage("Password must contain at leat one lowercase letter")
                .Matches(@"\d")
                .WithMessage("Password must contain at leat one number")
                .Matches(@"[^\w]")
                .WithMessage("Password must contain at leat one special character");
        }
    }
}
