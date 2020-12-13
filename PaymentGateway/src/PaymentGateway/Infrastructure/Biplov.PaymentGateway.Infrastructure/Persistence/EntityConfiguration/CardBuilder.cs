using Biplov.PaymentGateway.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Biplov.PaymentGateway.Infrastructure.Persistence.EntityConfiguration
{
    public class CardBuilder : IEntityTypeConfiguration<Card>
    {
        public void Configure(EntityTypeBuilder<Card> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasIndex(x => x.CardToken)
                .IsUnique();

            builder.Property(x => x.CardToken)
                .HasMaxLength(40);

            builder.Ignore(x => x.DomainEvents);

            builder.Property(x => x.Number)
                .IsRequired()
                .HasMaxLength(19);

            builder.Property(x => x.MaskedCardNumber)
                .HasMaxLength(19);

            builder.Property(x => x.ExpiryMonth)
                .IsRequired();

            builder.Property(x => x.Cvv)
                .HasMaxLength(4);
        }
    }
}
