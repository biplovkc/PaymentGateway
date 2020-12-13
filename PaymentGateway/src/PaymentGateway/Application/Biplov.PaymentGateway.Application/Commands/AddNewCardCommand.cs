using System;
using System.Runtime.Serialization;
using Biplov.Common.Core;
using Biplov.PaymentGateway.Application.Constants;
using Biplov.PaymentGateway.Application.Response;
using MediatR;

namespace Biplov.PaymentGateway.Application.Commands
{
    [DataContract]
    public class AddNewCardCommand : Command
    {
        [DataMember]
        public string Name { get; }

        [DataMember]
        public string Number { get; }

        [DataMember]
        public int ExpiryMonth { get; }

        [DataMember]
        public int ExpiryYear { get; }

        [DataMember]
        public string Cvv { get; }

        [DataMember]
        public override string CommandId { get; }

        public AddNewCardCommand(string name, string number, int expiryMonth, int expiryYear, string cvv, string commandId = null, string correlationId = null) : base(correlationId)
        {
            Name = name;
            Number = number;
            ExpiryMonth = expiryMonth;
            ExpiryYear = expiryYear;
            Cvv = cvv;
            CommandId = !string.IsNullOrWhiteSpace(commandId) 
                ? commandId 
                : $"{CommandPrefix.AddCard}{Number}-{DateTimeOffset.Now.ToUnixTimeSeconds()}";
        }
    }
}
