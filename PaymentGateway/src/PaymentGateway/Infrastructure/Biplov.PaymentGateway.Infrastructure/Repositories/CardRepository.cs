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
    public class CardRepository : ICardRepository
    {
        private readonly PaymentContext _dbContext;

        public CardRepository(PaymentContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IUnitOfWork UnitOfWork => _dbContext;
        public async Task<Card> GetByIdAsync(Guid id)
        {
            using (Log.Logger.TimeOperation("getting card with id : {id}", id))
            {
                return await _dbContext.Cards
                    .SingleOrDefaultAsync(x => x.Id.Equals(id));
            }
        }

        public Card Add(Card card)
        {
            using (Log.Logger.TimeOperation("adding card with id : {id}", card.Id))
            {
                return _dbContext.Cards.Add(card).Entity;
            }
        }

        public Card Update(Card card)
        {
            using (Log.Logger.TimeOperation("adding card with id : {id}", card.Id))
            {
                return _dbContext.Cards.Update(card).Entity;
            }
        }
    }
}
