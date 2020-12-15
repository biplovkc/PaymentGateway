using System;
using System.Collections.Generic;
using System.Linq;
using Biplov.Common.Core;
using Biplov.PaymentGateway.Domain.Enum;
using Biplov.PaymentGateway.Domain.Events;

namespace Biplov.PaymentGateway.Domain.Entities
{
    public class Payment : Entity
    {
        private Payment(){}
        
        public string PaymentId { get; } 

        public Guid MerchantIdentityId { get; }

        /// <summary>
        /// Id payment source.
        /// </summary>
        public IdPaymentSource Source { get; private set; }

        /// <summary>
        /// Payment currency
        /// </summary>
        public string Currency { get; }

        public decimal Amount { get; }

        public PaymentStatus Status { get; private set; }

        /// <summary>
        /// Reference provided by merchant
        /// </summary>
        public string Reference { get; }


        /// <summary>
        /// Request origin. Can be client's request ip to merchant, which is forwarded to us.
        /// </summary>
        public string RequestIp { get; }

        public string Description { get; }

        /// <summary>
        /// Payment billing address
        /// </summary>
        public Address BillingAddress { get; private set; }

        /// <summary>
        /// Payment recipient
        /// </summary>
        public PaymentRecipient Recipient { get; private set; }

        public DateTimeOffset RequestedAt { get; }

        /// <summary>
        /// Currently only supports url. These url should be set on merchant side
        /// </summary>
        public Notification MerchantNotification { get; private set; }


        private readonly List<MetaData> _metaData = new List<MetaData>();

        /// <summary>
        /// Additional metadata for given payment
        /// </summary>
        public IReadOnlyCollection<MetaData> MetaData => _metaData.AsReadOnly();
        
        /// <summary>
        /// Merchant
        /// </summary>
        public Merchant Merchant { get; }

        public Payment(Guid merchantId, string currency, decimal amount, string reference, string requestIp, string description)
        {
            if (currency.Length != 3)
                throw new ArgumentException("currency_length_must_be_3_letters");

            if (amount < 1)
                throw new ArgumentException("invalid_amount");

            PaymentId = $"payid_{Guid.NewGuid():N}";
            MerchantIdentityId = merchantId;
            Currency = currency;
            Amount = amount;
            Reference = reference;
            RequestIp = requestIp;
            Description = description;
            RequestedAt = DateTimeOffset.UtcNow;
        }

        public void AddBillingAddress(string addressLine1, string addressLine2, string city, string zipCode,
            string state, string country)
        {
            BillingAddress = new Address(addressLine1, addressLine2, city, zipCode, state, country);
        }


        public void SetCardPaymentSource(string cardId, string cvv)
        {
            if (!(cvv.Length == 3 || cvv.Length == 4))
                throw new ArgumentException("invalid_cvv");
            // Add validations
            Source = new IdPaymentSource(cardId, cvv);
        }

        public void SetNotificationsForMerchant(Uri successAddress, Uri errorAddress)
        {
            MerchantNotification = new Notification(successAddress, errorAddress);
        }

        public void SetPaymentRecipient(DateTime dateOfBirth, string accountNumber, string firstName, string lastName, string zipCode)
        {
            Recipient = new PaymentRecipient(dateOfBirth, accountNumber, firstName, lastName, zipCode);
        }

        public void SetMetaData(List<MetaData> metaData)
        {
            foreach (var data in metaData.Where(data => !_metaData.Contains(data)))
                _metaData.Add(data);
        }


        // TODO : Verify all the parameters are valid
        public void InitiatePayment()
        {
            Status = PaymentStatus.InProcess;
            RaiseDomainEvent(new PaymentInitiatedDomainEvent(PaymentId, Source.CardToken, Currency, Amount, Source, Recipient, BillingAddress, Description));
        }

        public void ChangePaymentStatus(PaymentStatus status)
        {
            Status = status;
        }

    }
}
