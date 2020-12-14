using System.Threading.Tasks;
using Biplov.Common.Core;
using Biplov.PaymentGateway.Domain.Entities;

namespace Biplov.PaymentGateway.Domain.Interfaces
{
    public interface IPaymentRepository : IRepository<Payment>
    {
        Task<Payment> GetByPaymentIdAsync(string paymentId);

        Payment Add(Payment payment);

        Payment Update(Payment payment);
    }
}
