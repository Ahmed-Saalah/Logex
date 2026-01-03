using FluentValidation;
using Logex.API.Models;

namespace Logex.API.Validators.Payments
{
    public class PaymentValidator : AbstractValidator<Payment>
    {
        public PaymentValidator()
        {
            RuleFor(p => p.Amount)
                .GreaterThan(0)
                .WithMessage("Payment amount must be greater than zero.");

            RuleFor(p => p.CardNumber).CreditCard().WithMessage("Invalid credit card number.");

            RuleFor(p => p.CVC).Matches(@"^\d{3,4}$").WithMessage("CVC must be 3 or 4 digits.");
        }
    }
}
