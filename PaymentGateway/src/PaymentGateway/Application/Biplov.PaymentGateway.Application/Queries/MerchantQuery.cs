using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Biplov.Common.Core;
using Biplov.PaymentGateway.Infrastructure.Persistence;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using Serilog;

using SerilogTimings.Extensions;

namespace Biplov.PaymentGateway.Application.Queries
{
    public class MerchantQuery : IMerchantQuery
    {
        private readonly PaymentContext _db;

        public MerchantQuery(PaymentContext db)
        {
            _db = db;
        }

        public async Task<Guid> GetMerchantIdAsync(string privateKey)
        {
            using (Log.Logger.TimeOperation("getting merchant id from privateKey: {privateKey}", privateKey))
            {
                var merchant = await _db.Merchants
                    .AsNoTracking()
                    .SingleOrDefaultAsync(x => x.PrivateKey.Equals(privateKey));
                return merchant?.MerchantIdentity ?? Guid.Empty;
            }
        }

        public async Task<bool> IsCurrencySupported(Guid merchantId, string currencyCode)
        {
            using (Log.Logger.TimeOperation("checking if currency : {currencyCode} is supported for merchant : {id}",currencyCode, merchantId))
            {
                var merchant = await _db.Merchants
                    .AsNoTracking()
                    .SingleOrDefaultAsync(x => x.MerchantIdentity == merchantId);

                return merchant.SupportedCurrencies.Contains(currencyCode.ToUpper());
            }
        }
    }
}
