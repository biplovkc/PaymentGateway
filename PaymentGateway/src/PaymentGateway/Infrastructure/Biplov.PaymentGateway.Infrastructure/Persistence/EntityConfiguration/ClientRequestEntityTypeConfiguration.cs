using Biplov.PaymentGateway.Infrastructure.Idempotency;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Biplov.PaymentGateway.Infrastructure.Persistence.EntityConfiguration
{
    class ClientRequestEntityTypeConfiguration
        : IEntityTypeConfiguration<ClientRequest>
    {
        public void Configure(EntityTypeBuilder<ClientRequest> requestConfiguration)
        {
            requestConfiguration.ToTable("Requests");

            requestConfiguration.HasKey(cr => cr.Id);

            requestConfiguration.Property(cr => cr.Name)
                .IsRequired()
                .HasColumnType("varchar(250)");

            requestConfiguration.Property(cr => cr.Time).IsRequired();
        }
    }
}
