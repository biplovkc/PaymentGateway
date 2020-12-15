using System.Threading.Tasks;
using Biplov.PaymentGateway.Application.Response;
using Card = Biplov.PaymentGateway.Application.Models.Card;

namespace Biplov.PaymentGateway.Application.Queries
{
    public interface ICardQuery
    {
        Task<CreateCardResponse> GetCardByCardNumberAsync(string cardNumber);
        Task<bool> IsCardValid(string cardToken, string cvv);
        Task<Card> GetCardAsync(string cardToken);
    }
}
