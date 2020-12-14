using Autofac;
using Biplov.BankService;
using Biplov.MockRiskAnalysis;
using Biplov.PaymentGateway.Application.Queries;
using Biplov.PaymentGateway.Domain.Interfaces;
using Biplov.PaymentGateway.Infrastructure.Idempotency;
using Biplov.PaymentGateway.Infrastructure.Repositories;
using Biplov.RiskAnalysis;

namespace Biplov.PaymentGateway.Application.IoC
{
    public class ApplicationModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<MerchantRepository>()
                .As<IMerchantRepository>()
                .InstancePerLifetimeScope();

            builder.RegisterType<CardRepository>()
                .As<ICardRepository>()
                .InstancePerLifetimeScope();

            builder.RegisterType<PaymentRepository>()
                .As<IPaymentRepository>()
                .InstancePerLifetimeScope();

            builder.RegisterType<MerchantQuery>()
                .As<IMerchantQuery>()
                .InstancePerLifetimeScope();

            builder.RegisterType<CardQuery>()
                .As<ICardQuery>()
                .InstancePerLifetimeScope();

            builder.RegisterType<MockRiskAnalysisService>()
                .As<IRiskAnalysisService>()
                .InstancePerLifetimeScope();

            builder.RegisterType<MockBank.MockBank>()
                .As<IBankService>()
                .InstancePerLifetimeScope();

            builder.RegisterType<RequestManager>()
                .As<IRequestManager>()
                .InstancePerLifetimeScope();
        }
    }
}
