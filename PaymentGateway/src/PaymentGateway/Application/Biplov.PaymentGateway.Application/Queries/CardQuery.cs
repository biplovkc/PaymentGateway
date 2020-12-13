using System;
using System.Threading.Tasks;
using Biplov.PaymentGateway.Application.Models;
using Biplov.PaymentGateway.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Biplov.PaymentGateway.Application.Queries
{
    public class CardQuery : ICardQuery
    {
        private readonly PaymentContext _db;

        public CardQuery(PaymentContext db)
        {
            _db = db;
        }

        public async Task<bool> IsCardValid(string cardToken, string cvv)
        {
            var card = await _db.Cards
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.CardToken.Equals(cardToken) && x.Cvv.Equals(cvv));
            if (card.ExpiryYear < DateTime.Now.Year && (card.ExpiryYear == DateTime.Now.Year && card.ExpiryYear < DateTime.Now.Month))
            {
                return false;
            }
            return true;
        }

        public async Task<Card> GetCardAsync(string cardToken)
        {
            var card = await _db.Cards
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.CardToken.Equals(cardToken));

            return new Card
            {
                Number = card.Number,
                Cvv = card.Cvv,
                ExpiryYear = card.ExpiryYear,
                ExpiryMonth = card.ExpiryMonth,
                Name = card.Name
            };
        }
    }
}
