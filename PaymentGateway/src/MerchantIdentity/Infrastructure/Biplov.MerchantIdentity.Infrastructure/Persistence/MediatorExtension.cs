﻿using System.Linq;
using System.Threading.Tasks;
using Biplov.Common.Core;
using MediatR;

namespace Biplov.MerchantIdentity.Infrastructure.Persistence
{
    public static class MediatorExtension
    {
        public static async Task DispatchDomainEventAsync(this IMediator mediator, MerchantIdentityContext dbIdentityContext)
        {
            var domainEntities = dbIdentityContext.ChangeTracker
                .Entries<Entity>()
                .Where(x => x.Entity.DomainEvents != null && x.Entity.DomainEvents.Any());

            var entityEntries = domainEntities.ToList();
            var domainEvents = entityEntries
                .SelectMany(x => x.Entity.DomainEvents)
                .ToList();

            entityEntries.ToList()
                .ForEach(entity => entity.Entity.ClearDomainEvents());

            foreach (var domainEvent in domainEvents)
                await mediator.Publish(domainEvent);
        }

        public static void DispatchDomainEvent(this IMediator mediator, MerchantIdentityContext dbIdentityContext)
        {
            var domainEntities = dbIdentityContext.ChangeTracker
                .Entries<Entity>()
                .Where(x => x.Entity.DomainEvents != null && x.Entity.DomainEvents.Any());

            var entityEntries = domainEntities.ToList();
            var domainEvents = entityEntries
                .SelectMany(x => x.Entity.DomainEvents)
                .ToList();

            entityEntries.ToList()
                .ForEach(entity => entity.Entity.ClearDomainEvents());

            foreach (var domainEvent in domainEvents)
                mediator.Publish(domainEvent);
        }
    }
}
