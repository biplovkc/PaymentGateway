using System;
using System.Collections.Generic;
using System.Linq;
using Biplov.Common.Core;
using Biplov.PaymentGateway.Domain.Enum;

namespace Biplov.PaymentGateway.Domain.Entities
{
    public class Payment 
    {
        private Payment(){}

        public Guid MerchantId { get; }

        /// <summary>
        /// Source of payment.
        /// </summary>
        public PaymentSource Source { get; private set; }

        /// <summary>
        /// Payment currency
        /// </summary>
        public string Currency { get; }

        public int Amount { get; }

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

        public Payment(Guid merchantId, string currency, int amount, string reference, string requestIp, string description)
        {
            MerchantId = merchantId;
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


        public void SetIdPaymentSource(Guid paymentIdSource)
        {
            throw new NotImplementedException();
        }

        public void SetCardPaymentSource(string name, string number, int expiryYear, int expiryMonth, string cvv, string phoneNumber)
        {
            // Add validations
            Source = new CardPaymentSource(number, expiryMonth, expiryYear, name, cvv, phoneNumber);
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

        public void InitiatePayment()
        {
            Status = PaymentStatus.InProcess;
            //RaiseDomainEvent(new PaymentStartedDomainEvent(Id, Currency, Amount, Source, Recipient, BillingAddress, Description));
        }

        public void ChangePaymentStatus(PaymentStatus status)
        {
            Status = status;
            //RaiseDomainEvent(new PaymentStatusUpdatedDomainEvent(Id, Status, MerchantNotification));
        }
    }
}
