using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Biplov.PaymentGateway.Infrastructure.Persistence
{
    public class PaymentContextSeed
    {
        public async Task Seed(PaymentContext context, ILogger<PaymentContextSeed> logger, int? retry = 5)
        {
            var retryForAvailability = retry ?? 0;
            try
            {
                if (!(await context.Database.GetAppliedMigrationsAsync()).Any())
                {
                    await context.Database.EnsureCreatedAsync();
                }
            }
            catch (Exception e)
            {
                await Task.Delay(2000);
                if (retryForAvailability < 10)
                {
                    retryForAvailability++;
                    logger.LogError(e, "EXCEPTION ERROR while migrating {DbContextName}", nameof(PaymentContext));
                    await Seed(context, logger, retryForAvailability);
                }
            }
        }
    }
}
