using System.Threading.Tasks;
using Card = Biplov.PaymentGateway.Application.Models.Card;

namespace Biplov.PaymentGateway.Application.Queries
{
    public interface ICardQuery
    {
        Task<bool> IsCardValid(string cardToken, string cvv);
        Task<Card> GetCardAsync(string cardToken);
    }
}
