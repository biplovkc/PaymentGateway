using System;
using System.Threading.Tasks;
using Biplov.Common.Core;
using Biplov.PaymentGateway.Application.Constants;
using Biplov.PaymentGateway.Application.Dtos;
using Biplov.PaymentGateway.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Biplov.PaymentGateway.Application.Queries
{
    public class PaymentQuery : IPaymentQuery
    {
        private readonly PaymentContext _db;
        private readonly ILogger<PaymentQuery> _logger;

        public PaymentQuery(PaymentContext db, ILogger<PaymentQuery> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task<Result<PaymentDto>> GetPaymentInfoAsync(string paymentId)
        {
            try
            {
                var payment = await _db.Payments
                    .AsNoTracking()
                    .Include(x=>x.Source)
                    .SingleOrDefaultAsync(x => x.PaymentId.Equals(paymentId));

                if (payment is null)
                {
                    return Result.Fail<PaymentDto>(ExternalErrorReason.PaymentNotFound);
                }

                return Result.Ok(new PaymentDto
                {
                    Amount = payment.Amount,
                    Currency = payment.Currency,
                    PaymentId = paymentId,
                    PaymentSource = payment.Source.CardToken,
                    CreatedAt = payment.RequestedAt,
                    Status = payment.Status
                });
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error fetching payment id {paymentId}", paymentId);
                return Result.Fail<PaymentDto>(e.Message);
            }
        }
    }
}
