using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Biplov.PaymentGateway.Infrastructure.Persistence
{
    public class PaymentContextDesignFactory : IDesignTimeDbContextFactory<PaymentContext>
    {
        public PaymentContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<PaymentContext>();
            optionsBuilder.UseSqlServer();
            return new PaymentContext(optionsBuilder.Options, new NoMediator());
        }

        private class NoMediator: IMediator
        {
            public Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = new CancellationToken())
                => Task.FromResult<TResponse>(default);

            public Task<object> Send(object request, CancellationToken cancellationToken = new CancellationToken())
                => Task.FromResult<object>(default);

            public Task Publish(object notification, CancellationToken cancellationToken = new CancellationToken())
                => Task.CompletedTask;

            public Task Publish<TNotification>(TNotification notification, CancellationToken cancellationToken = new CancellationToken()) where TNotification : INotification
                => Task.CompletedTask;
        }
    }
}
