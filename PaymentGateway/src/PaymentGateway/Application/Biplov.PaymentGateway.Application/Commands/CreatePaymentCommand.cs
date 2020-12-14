using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Biplov.Common.Core;
using Biplov.PaymentGateway.Application.Constants;
using Biplov.PaymentGateway.Application.Models;
using MediatR;

namespace Biplov.PaymentGateway.Application.Commands
{
    [DataContract]
    public class CreatePaymentCommand : Command, IRequest<Result>
    {
        [DataMember]
        public Guid MerchantId { get; set; }

        [DataMember]
        public string CardToken { get; }

        [DataMember]
        public string Cvv { get; }

        [DataMember]
        public decimal Amount { get; }

        [DataMember]
        public string Currency { get; }

        [DataMember]
        public Shipping Shipping { get; }

        [DataMember]
        public PaymentRecipient Recipient { get; }

        [DataMember]
        public string Reference { get; }

        [DataMember]
        public string Description { get; }

        [DataMember]
        public string OriginIp { get; }

        [DataMember]
        public Uri SuccessUrl { get; }

        [DataMember]
        public Uri ErrorUrl{get; }

        [DataMember]
        public Dictionary<string, string> MetaData { get; }

        [DataMember]
        public override string CommandId =>
            $"{CommandPrefix.CreatePayment}{CardToken}-{DateTimeOffset.Now.ToUnixTimeSeconds()}";

        public CreatePaymentCommand(Guid merchantId, string cardToken, string cvv, decimal amount, string currency, Shipping shipping, PaymentRecipient recipient,
            string reference, string description, string originIp, Uri successUrl, Uri errorUrl, Dictionary<string, string> metaData, 
            string correlationId = null) : base(correlationId)
        {
            MerchantId = merchantId;
            CardToken = cardToken;
            Cvv = cvv;
            Amount = amount;
            Currency = currency;
            Shipping = shipping;
            Reference = reference;
            Description = description;
            OriginIp = originIp;
            SuccessUrl = successUrl;
            ErrorUrl = errorUrl;
            MetaData = metaData;
        }
    }
}
