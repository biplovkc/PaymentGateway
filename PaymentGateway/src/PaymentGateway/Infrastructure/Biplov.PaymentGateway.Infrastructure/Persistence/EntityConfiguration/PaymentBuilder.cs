using System;
using Biplov.PaymentGateway.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Biplov.PaymentGateway.Infrastructure.Persistence.EntityConfiguration
{
    public class PaymentBuilder : IEntityTypeConfiguration<Payment>
    {
        public void Configure(EntityTypeBuilder<Payment> builder)
        {
            throw new NotImplementedException();
        }
    }
}
