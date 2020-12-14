using System.Threading.Tasks;
using Biplov.Common.Core;

namespace Biplov.BankService
{
    public interface IBankService
    {
        Task<Result> ValidateCard(string cardHolderName, string cardNumber, int expiryMonth, int expiryYear,
            string cvv);

        Task<Result> HandlePaymentAsync(decimal amount, string currency, PaymentRecipient recipient, string description, Card card);
    }
}
