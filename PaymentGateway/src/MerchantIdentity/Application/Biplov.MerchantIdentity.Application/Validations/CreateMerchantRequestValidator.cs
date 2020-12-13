using System.Linq;
using Biplov.Common.Core.Utilities;
using Biplov.MerchantIdentity.Application.Requests;
using FluentValidation;

namespace Biplov.MerchantIdentity.Application.Validations
{
    public class CreateMerchantRequestValidator: AbstractValidator<CreateMerchantRequest>
    {
        public CreateMerchantRequestValidator()
        {
            CascadeMode = CascadeMode.Stop;

            RuleFor(x => x.Email)
                .NotEmpty()
                .Must(IsValidEmail)
                .WithErrorCode("invalid_email_address");

            RuleFor(x => x.Name)
                .NotEmpty()
                .MinimumLength(2)
                .WithErrorCode("invalid_merchant_name");

            RuleFor(x => x.SupportedCurrencies)
                .NotEmpty()
                .Must(AreValidCurrencies)
                .WithErrorCode("invalid_currency_code");
        }

        private bool AreValidCurrencies(string currencies)
        {
            var trimmedCurrencies = new string(currencies.ToCharArray().Where(x => !char.IsWhiteSpace(x)).ToArray());
            var currenciesArray = trimmedCurrencies.Split(',');
            return currenciesArray.All(currency => currency.Length == 3);
        }

        private bool IsValidEmail(string arg) => arg.IsValidEmail();
    }
}
