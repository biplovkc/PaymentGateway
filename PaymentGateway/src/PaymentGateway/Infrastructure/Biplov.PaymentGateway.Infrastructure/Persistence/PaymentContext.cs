using System;
using System.Threading;
using System.Threading.Tasks;
using Biplov.Common.Core;
using Biplov.PaymentGateway.Domain.Entities;
using Biplov.PaymentGateway.Infrastructure.Persistence.EntityConfiguration;
using MediatR;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Serilog;

namespace Biplov.PaymentGateway.Infrastructure.Persistence
{
    public class PaymentContext : DbContext, IUnitOfWork
    {

        public DbSet<Merchant> Merchants { get; set; }

        private readonly IMediator _mediator;
        public static readonly ILoggerFactory DbLoggerFactory
            = LoggerFactory.Create(builder => { builder.AddSerilog(); });
        public PaymentContext(DbContextOptions<PaymentContext> options) :
            base(options)
        {
        }

        public PaymentContext(DbContextOptions<PaymentContext> options, IMediator mediator) :
            base(options)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));

            System.Diagnostics.Debug.WriteLine($"DataCollectionDbContext -> {nameof(this.GetHashCode)}");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var assembly = typeof(PaymentBuilder).Assembly;
            modelBuilder.ApplyConfigurationsFromAssembly(assembly);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder
                .UseLoggerFactory(DbLoggerFactory);

        public async Task<Result> SaveEntitiesAsync(CancellationToken cancellationToken = default)
        {
            // Dispatch Domain Events collection. 
            // Choices:
            // A) Right BEFORE committing data (EF SaveChanges) into the DB will make a single transaction including  
            // side effects from the domain event handlers which are using the same DbContext with "InstancePerLifetimeScope" or "scoped" lifetime
            // B) Right AFTER committing data (EF SaveChanges) into the DB will make multiple transactions. 
            // You will need to handle eventual consistency and compensatory actions in case of failures in any of the Handlers. 

            // After executing this line all the changes (from the Command Handler and Domain Event Handlers) 
            // performed through the DbContext will be committed
            try
            {
                await _mediator.DispatchDomainEventAsync(this);
                await base.SaveChangesAsync(cancellationToken);
                return Result.Ok();
            }
            catch (Exception e)
            {
                var logger = DbLoggerFactory.CreateLogger(nameof(PaymentContextDesignFactory));
                logger.LogError(e, e.Message);
                return Result.Ok(e.Message);
            }
        }
        public override int SaveChanges()
        {
            _mediator.DispatchDomainEvent(this);
            return base.SaveChanges();
        }
    }
}
