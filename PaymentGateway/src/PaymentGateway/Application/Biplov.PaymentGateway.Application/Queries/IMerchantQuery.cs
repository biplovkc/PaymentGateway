using System;
using System.Threading.Tasks;
using Biplov.Common.Core;

namespace Biplov.PaymentGateway.Application.Queries
{
    public interface IMerchantQuery
    {
        Task<Guid> GetMerchantIdAsync(string privateKey);

        Task<bool> IsCurrencySupported(Guid merchantId, string currencyCode);
    }
}
