using System.Threading.Tasks;

namespace Biplov.PaymentGateway.Infrastructure.Idempotency
{
    public interface IRequestManager
    {
        Task<bool> ExistAsync(string id);

        Task CreateRequestForCommandAsync<T>(string id);
    }
}
