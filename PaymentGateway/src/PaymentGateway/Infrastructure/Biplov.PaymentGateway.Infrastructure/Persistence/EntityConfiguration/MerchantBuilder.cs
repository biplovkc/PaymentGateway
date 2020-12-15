using System.Collections.Generic;
using Biplov.PaymentGateway.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;

namespace Biplov.PaymentGateway.Infrastructure.Persistence.EntityConfiguration
{
    public class MerchantBuilder : IEntityTypeConfiguration<Merchant>
    {
        public void Configure(EntityTypeBuilder<Merchant> builder)
        {
            builder.ToTable("Merchants");

            builder.HasKey(x => x.Id);

            builder.Ignore(x => x.DomainEvents);

            builder.HasIndex(x => x.MerchantIdentity)
                .IsUnique();

            builder.Property(x => x.Email)
                .HasColumnName("Email");

            builder.Property(x => x.PublicKey)
                .HasColumnName("PublicKey");

            builder.Property(x => x.PrivateKey)
                .HasColumnName("PrivateKey");

            builder.Property(x => x.SupportedCurrencies)
                .HasField("_supportedCurrencies")
                .HasConversion(
                    v => JsonConvert.SerializeObject(v,
                        new JsonSerializerSettings {NullValueHandling = NullValueHandling.Ignore}),
                    v => JsonConvert.DeserializeObject<List<string>>(v,
                        new JsonSerializerSettings {NullValueHandling = NullValueHandling.Ignore}));
        }
    }
}
