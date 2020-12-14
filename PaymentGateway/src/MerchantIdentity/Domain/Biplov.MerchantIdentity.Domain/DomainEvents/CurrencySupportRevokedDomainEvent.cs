using System;
using Biplov.Common.Core;
using MediatR;

namespace Biplov.MerchantIdentity.Domain.DomainEvents
{
    public class CurrencySupportRevokedDomainEvent: DomainEvent, INotification
    {
        public Guid MerchantId { get; }
        public string CurrencyCode { get; }
        public string CommandId { get; }

        public CurrencySupportRevokedDomainEvent(Guid merchantId, string currencyCode, string commandId)
        {
            MerchantId = merchantId;
            CurrencyCode = currencyCode;
            CommandId = commandId;
        }
    }
}
