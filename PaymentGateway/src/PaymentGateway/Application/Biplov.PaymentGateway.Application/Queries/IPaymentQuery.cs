using System.Threading.Tasks;
using Biplov.Common.Core;
using Biplov.PaymentGateway.Application.Dtos;

namespace Biplov.PaymentGateway.Application.Queries
{
    public interface IPaymentQuery
    {
        Task<Result<PaymentDto>> GetPaymentInfoAsync(string paymentId);
    }
}
