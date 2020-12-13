using System;
using System.Threading;
using System.Threading.Tasks;
using Biplov.Common.Core;
using Biplov.MerchantIdentity.Domain.Entities;
using Biplov.MerchantIdentity.Infrastructure.Persistence.EntityConfiguration;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Serilog;

namespace Biplov.MerchantIdentity.Infrastructure.Persistence
{
    public class MerchantIdentityContext: DbContext, IUnitOfWork
    {
        public DbSet<Merchant> Merchants { get; set; }

        private readonly IMediator _mediator;
        public static readonly ILoggerFactory DbLoggerFactory
            = LoggerFactory.Create(builder => { builder.AddSerilog(); });

        public MerchantIdentityContext(DbContextOptions<MerchantIdentityContext> options) :
            base(options)
        {
        }

        public MerchantIdentityContext(DbContextOptions<MerchantIdentityContext> options, IMediator mediator) :
            base(options)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));

            System.Diagnostics.Debug.WriteLine($"DataCollectionDbContext -> {nameof(GetHashCode)}");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var assembly = typeof(MerchantBuilder).Assembly;
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
                var logger = DbLoggerFactory.CreateLogger(nameof(MerchantContextDesignFactory));
                logger.LogError(e, e.Message);
                return Result.Fail(e.Message);
            }
        }
    }
}
