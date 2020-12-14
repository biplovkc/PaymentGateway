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
    public class PaymentRepository : IPaymentRepository
    {
        private readonly PaymentContext _dbContext;
        public IUnitOfWork UnitOfWork => _dbContext;

        public PaymentRepository(PaymentContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Payment> GetByPaymentIdAsync(string paymentId)
        {
            using (Log.Logger.TimeOperation("getting payment with id : {paymentId}", paymentId))
            {
                return await _dbContext.Payments
                    .SingleOrDefaultAsync(x => x.PaymentId.Equals(paymentId));
            }
        }

        public Payment Add(Payment payment)
        {
            using (Log.Logger.TimeOperation("adding payment with id : {paymentId}", payment.Id))
            {
                return _dbContext.Payments.Add(payment).Entity;
            }
        }

        public Payment Update(Payment payment)
        {
            using (Log.Logger.TimeOperation("updating payment with id : {paymentId}", payment.Id))
            {
                return _dbContext.Payments.Update(payment).Entity;
            }
        }
    }
}
