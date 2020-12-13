using Biplov.Common.Core;
using Biplov.PaymentGateway.Domain.Entities;

using MediatR;

namespace Biplov.PaymentGateway.Domain.Events
{
    public class PaymentInitiatedDomainEvent : DomainEvent, INotification
    {
        public PaymentInitiatedDomainEvent(string paymentId,string cardToken, string currency, decimal amount, IdPaymentSource source, PaymentRecipient recipient, Address billingAddress, string description)
        {
            PaymentId = paymentId;
            CardToken = cardToken;
            Currency = currency;
            Amount = amount;
            Source = source;
            Recipient = recipient;
            BillingAddress = billingAddress;
            Description = description;
        }

        public string PaymentId { get; }
        public string CardToken { get; }
        public string Currency { get; }
        public decimal Amount { get; }
        public IdPaymentSource Source { get; }
        public PaymentRecipient Recipient { get; }
        public Address BillingAddress { get; }
        public string Description { get; }
    }
}
