using System;
using Biplov.Common.Core;
using MediatR;

namespace Biplov.MerchantIdentity.Domain.DomainEvents
{
    public class CurrencySupportAddedDomainEvent : DomainEvent, INotification
    {
        public Guid MerchantId { get; }
        public string CurrencyCode { get; }
        public string CommandId { get; }
        public CurrencySupportAddedDomainEvent(Guid id, string currency, string commandId = null)
        {
            MerchantId = id;
            CurrencyCode = currency;
            CommandId = commandId;
        }
    }
}
