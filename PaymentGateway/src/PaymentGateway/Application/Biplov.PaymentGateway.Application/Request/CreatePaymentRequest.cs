using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Biplov.PaymentGateway.Application.Models;
using MediatR;

namespace Biplov.PaymentGateway.Application.Request
{

    [DataContract]
    public class CreatePaymentRequest : IRequest<bool>
    {
        // Todo : add support for different payment types
        //[DataMember]
        //public PaymentType Type { get; set; }
        [DataMember]
        public string CardToken { get; set; }

        [DataMember]
        public string Cvv { get; set; }

        [DataMember]
        public decimal Amount { get;set; }

        [DataMember]
        public string Currency { get; set; }

        [DataMember]
        public Shipping Shipping { get;set; }

        [DataMember]
        public PaymentRecipient Recipient { get;set; }

        [DataMember]
        public string Reference { get; set; }

        [DataMember]
        public string Description { get;set; }

        [DataMember]
        public string OriginIp { get;set; }

        [DataMember]
        public Uri SuccessUrl { get; set; }

        [DataMember]
        public Uri ErrorUrl{get; set; }

        [DataMember]
        public Dictionary<string, string> MetaData { get;set; }
    }
}
