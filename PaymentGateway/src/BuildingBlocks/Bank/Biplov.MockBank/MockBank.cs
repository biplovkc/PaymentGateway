using System;
using System.Threading.Tasks;
using Biplov.BankService;
using Biplov.Common.Core;

namespace Biplov.MockBank
{
    public class MockBank : IBankService
    {
        private readonly string[] _reasons = new[] {"insufficient_funds", "card_invalid", "card_blocked"};

        /// <summary>
        /// Validates card info.
        /// </summary>
        /// <param name="cardHolderName"></param>
        /// <param name="cardNumber"></param>
        /// <param name="expiryMonth"></param>
        /// <param name="expiryYear"></param>
        /// <param name="cvv"></param>
        /// <returns>Result class with ok result if card is valid else returns fail</returns>
        public Task<Result> ValidateCard(string cardHolderName, string cardNumber, int expiryMonth, int expiryYear, string cvv)
        {
            var random = new Random();
            var probability = random.Next(100);

            // Return true with 99 percent probability
            var validity = probability <= 99;
            return Task.FromResult(validity ? Result.Ok() : Result.Fail("invalid_card"));
        }

        /// <summary>
        /// Handle payment
        /// </summary>
        /// <param name="amount">amount to be paid out</param>
        /// <param name="currency">currency</param>
        /// <param name="recipient">payment recipient</param>
        /// <param name="description">payment description</param>
        /// <param name="card">card detail</param>
        /// <returns></returns>
        public Task<Result> HandlePaymentAsync(decimal amount, string currency, PaymentRecipient recipient, string description, Card card)
        {
            var random = new Random();
            var probability = random.Next(100);

            // Return true with 99 percent probability
            var validity = probability <= 90;

            var errorReason = random.Next(_reasons.Length);
            return Task.FromResult(validity ? Result.Ok() : Result.Fail(_reasons[errorReason]));
        }
    }
}
