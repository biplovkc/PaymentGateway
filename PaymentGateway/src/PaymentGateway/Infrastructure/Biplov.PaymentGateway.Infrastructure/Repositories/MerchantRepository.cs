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
        private readonly PaymentContext _dbContext;

        public MerchantRepository(PaymentContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public IUnitOfWork UnitOfWork => _dbContext;
        public async Task<Merchant> GetByIdentityIdAsync(Guid id)
        {
            using (Log.Logger.TimeOperation("getting merchant with id : {id}", id))
            {
                return await _dbContext.Merchants
                    .SingleOrDefaultAsync(x => x.MerchantIdentity.Equals(id));
            }
        }

        public async Task<Merchant> GetByEmailAsync(string email)
        {
            using (Log.Logger.TimeOperation("getting merchant with id : {email}", email))
            {
                return await _dbContext.Merchants
                    .SingleOrDefaultAsync(x => x.Email.Equals(email));
            }
        }

        public Merchant Add(Merchant merchant)
        {
            using (Log.Logger.TimeOperation("adding merchant with id : {id}", merchant.Id))
            {
                return _dbContext.Merchants.Add(merchant).Entity;
            }
        }

        public Merchant Update(Merchant merchant)
        {
            using (Log.Logger.TimeOperation("updating merchant with id : {id}", merchant.Id))
            {
                return _dbContext.Merchants.Update(merchant).Entity;
            }
        }
    }
}
