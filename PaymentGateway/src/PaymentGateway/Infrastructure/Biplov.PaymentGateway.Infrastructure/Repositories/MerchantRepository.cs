using System;
using System.Threading.Tasks;
using Biplov.Common.Core;
using Biplov.PaymentGateway.Domain.Entities;
using Biplov.PaymentGateway.Domain.Interfaces;
using Biplov.PaymentGateway.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Serilog;
using SerilogTimings.Extensions;

namespace Biplov.PaymentGateway.Infrastructure.Repositories
{
    public class MerchantRepository : IMerchantRepository
    {
        private readonly PaymentContext _dbIdentityContext;

        public MerchantRepository(PaymentContext dbIdentityContext)
        {
            _dbIdentityContext = dbIdentityContext ?? throw new ArgumentNullException(nameof(dbIdentityContext));
        }

        public IUnitOfWork UnitOfWork => _dbIdentityContext;
        public async Task<Merchant> GetByIdentityIdAsync(Guid id)
        {
            using (Log.Logger.TimeOperation("getting merchant with id : {id}", id))
            {
                return await _dbIdentityContext.Merchants
                    .SingleOrDefaultAsync(x => x.MerchantIdentity.Equals(id));
            }
        }

        public async Task<Merchant> GetByEmailAsync(string email)
        {
            using (Log.Logger.TimeOperation("getting merchant with id : {email}", email))
            {
                return await _dbIdentityContext.Merchants
                    .SingleOrDefaultAsync(x => x.Email.Equals(email));
            }
        }

        public Merchant Add(Merchant merchant)
        {
            using (Log.Logger.TimeOperation("adding merchant with id : {id}", merchant.Id))
            {
                return _dbIdentityContext.Merchants.Add(merchant).Entity;
            }
        }

        public Merchant Update(Merchant merchant)
        {
            using (Log.Logger.TimeOperation("updating merchant with id : {id}", merchant.Id))
            {
                return _dbIdentityContext.Merchants.Update(merchant).Entity;
            }
        }
    }
}
