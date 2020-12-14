using System;
using System.Threading.Tasks;
using Biplov.Common.Core;
using Biplov.PaymentGateway.Domain.Entities;

namespace Biplov.PaymentGateway.Domain.Interfaces
{
    public interface ICardRepository : IRepository<Card>
    {
        Task<Card> GetByIdAsync(Guid id);

        Card Add(Card card);

        Card Update(Card card);
    }
}
