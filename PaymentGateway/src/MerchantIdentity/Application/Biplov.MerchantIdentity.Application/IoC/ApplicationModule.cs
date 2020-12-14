using Autofac;
using Biplov.MerchantIdentity.Domain.Interfaces;
using Biplov.MerchantIdentity.Infrastructure.Repository;

namespace Biplov.MerchantIdentity.Application.IoC
{
    public class ApplicationModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<MerchantRepository>()
                .As<IMerchantIdentityRepository>()
                .InstancePerLifetimeScope();
        }
    }
}
