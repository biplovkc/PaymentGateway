using System;
using Biplov.PaymentGateway.Application.Constants;
using Biplov.PaymentGateway.Application.Request;
using FluentValidation;

namespace Biplov.PaymentGateway.Application.Validations
{
    public class CreatePaymentRequestValidator : AbstractValidator<CreatePaymentRequest>
    {
        public CreatePaymentRequestValidator()
        {
            RuleFor(x => x.Amount)
                .GreaterThan(1)
                .WithErrorCode(ExternalErrorReason.InvalidPaymentAmount);

            RuleFor(x => x.Currency)
                .MaximumLength(3)
                .WithErrorCode(ExternalErrorReason.InvalidCurrency);

            RuleFor(x => x.Recipient.AccountNumber)
                .NotEmpty()
                .MaximumLength(19)
                .WithErrorCode(ExternalErrorReason.InvalidRecepientAccountNumber);

            RuleFor(x => x.Recipient.DateOfBirth)
                .Must(IsValidDateOfBirth)
                .WithErrorCode(ExternalErrorReason.InvalidRecipientDateOfBirth);

            RuleFor(x => x.Recipient.FirstName)
                .NotEmpty()
                .WithErrorCode(ExternalErrorReason.InvalidRecipientFirstName);

            RuleFor(x => x.Recipient.LastName)
                .NotEmpty()
                .WithErrorCode(ExternalErrorReason.InvalidRecipientLastName);

            RuleFor(x => x.CardToken)
                .Length(40, 40)
                .WithErrorCode(ExternalErrorReason.InvalidCardToken);

            RuleFor(x => x.Cvv)
                .Length(3, 4)
                .WithErrorCode(ExternalErrorReason.InvalidCvv);

        }

        private bool IsValidDateOfBirth(string arg)
        {
            var result = DateTime.TryParse(arg, out DateTime dob);
            return result && dob.Year < DateTime.Now.Year;
        }
    }
}
