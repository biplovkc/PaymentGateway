using System;
using Biplov.PaymentGateway.Application.Constants;
using Biplov.PaymentGateway.Application.Request;
using FluentValidation;

namespace Biplov.PaymentGateway.Application.Validations
{
    public class TokenizeCardValidator : AbstractValidator<TokenizeCardRequest>
    {
        public TokenizeCardValidator()
        {
            RuleFor(x => x.Cvv)
                .NotEmpty()
                .MaximumLength(4)
                .MinimumLength(3)
                .WithErrorCode(ExternalErrorReason.InvalidCvv);

            RuleFor(x => x.Number)
                .NotEmpty()
                .Matches("[0-9]")
                .WithErrorCode(ExternalErrorReason.InvalidCardNumber);

            RuleFor(x => x.Number)
                .NotEmpty()
                .Length(8,19)
                .WithErrorCode(ExternalErrorReason.InvalidCardNumber);

            RuleFor(x => x.ExpiryMonth)
                .NotEmpty()
                .InclusiveBetween(1, 12)
                .WithErrorCode(ExternalErrorReason.InvalidCardExpiryMonth);

            RuleFor(x => x.ExpiryYear)
                .NotEmpty()
                .GreaterThanOrEqualTo(DateTime.Now.Year)
                .WithErrorCode(ExternalErrorReason.InvalidCardExpiryYear);

            RuleFor(x => x.ExpiryMonth)
                .GreaterThan(DateTime.Now.Month)
                .When(x => x.ExpiryYear == DateTime.Now.Year)
                .WithErrorCode(ExternalErrorReason.InvalidCardExpiryYear);
        }

    }
}
