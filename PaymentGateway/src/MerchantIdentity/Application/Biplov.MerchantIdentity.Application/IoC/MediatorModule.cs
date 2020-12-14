using System.Reflection;
using Autofac;
using Biplov.MerchantIdentity.Application.CommandHandlers;
using Biplov.MerchantIdentity.Application.Commands;
using Biplov.MerchantIdentity.Application.DomainEventHandlers;
using Biplov.MerchantIdentity.Domain.DomainEvents;
using MediatR;

namespace Biplov.MerchantIdentity.Application.IoC
{
    public class MediatorModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(typeof(IMediator).GetTypeInfo().Assembly)
                .AsImplementedInterfaces();
            
            // Register all the Command classes (they implement IRequestHandler) in assembly holding the Commands
            builder.RegisterAssemblyTypes(typeof(CreateMerchantCommandHandler).GetTypeInfo().Assembly)
                .AsClosedTypesOf(typeof(IRequestHandler<,>));

            // Register the DomainEventHandler classes (they implement INotificationHandler<>) in assembly holding the Domain Events
            builder.RegisterAssemblyTypes(typeof(MerchantAddedDomainEventHandler).GetTypeInfo().Assembly)
                .AsClosedTypesOf(typeof(INotificationHandler<>));

            builder.Register<ServiceFactory>(context =>
            {
                var componentContext = context.Resolve<IComponentContext>();
                return t => { return componentContext.TryResolve(t, out var o) ? o : null; };
            });
        }
    }
}
