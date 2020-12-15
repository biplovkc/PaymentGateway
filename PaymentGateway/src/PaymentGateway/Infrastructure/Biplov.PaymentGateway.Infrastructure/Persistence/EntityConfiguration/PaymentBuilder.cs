using Biplov.PaymentGateway.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Biplov.PaymentGateway.Infrastructure.Persistence.EntityConfiguration
{
    public class PaymentBuilder : IEntityTypeConfiguration<Payment>
    {
        public void Configure(EntityTypeBuilder<Payment> builder)
        {
            builder.ToTable("Payments");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.PaymentId)
                .HasColumnName("PaymentId");

            builder.Property(x => x.Status)
                .HasColumnName("Status");

            builder.HasOne(x => x.Merchant)
                .WithMany(x => x.Payments);

            builder.Ignore(x => x.DomainEvents);

            builder.Property(x => x.Currency)
                .HasColumnName("Currency")
                .HasMaxLength(3);

            builder.Property(x => x.Amount)
                .HasColumnName("Amount");

            builder.Property(x => x.Reference)
                .HasColumnName("Reference");

            builder.Property(x => x.MerchantIdentityId)
                .HasColumnName("MerchantIdentityId");

            builder.Property(x => x.RequestIp)
                .HasColumnName("RequestIp");

            builder.Property(x => x.Description)
                .HasColumnName("Description");

            builder.Property(x => x.RequestedAt)
                .HasColumnName("RequestedAt");

            builder.OwnsOne(x => x.Source, r =>
            {
                r.Property(x => x.CardToken)
                    .HasColumnName("CardToken")
                    .HasMaxLength(45);

                r.Property(x => x.Cvv)
                    .HasColumnName("Cvv")
                    .HasMaxLength(4);
            });

            var paymentSourceNavigation = builder.Metadata.FindNavigation(nameof(Payment.Source));
            paymentSourceNavigation.SetPropertyAccessMode(PropertyAccessMode.Field);

            builder.OwnsOne(x => x.BillingAddress, r =>
            {
                r.Property(x => x.AddressLine1)
                    .HasColumnName("AddressLine1")
                    .HasMaxLength(100);

                r.Property(x => x.AddressLine2)
                    .HasColumnName("AddressLine2")
                    .HasMaxLength(100);

                r.Property(x => x.ZipCode)
                    .HasColumnName("ZipCode")
                    .HasMaxLength(20);

                r.Property(x => x.State)
                    .HasColumnName("State")
                    .HasMaxLength(100);

                r.Property(x => x.City)
                    .HasColumnName("City")
                    .HasMaxLength(100);

                r.Property(x => x.Country)
                    .HasColumnName("Country")
                    .HasMaxLength(100);
            });

            var billingAddressNavigation = builder.Metadata.FindNavigation(nameof(Payment.BillingAddress));
            billingAddressNavigation.SetPropertyAccessMode(PropertyAccessMode.Field);

            builder.OwnsOne(x => x.Recipient, r =>
            {
                r.Property(x => x.AccountNumber)
                    .HasColumnName("RecipientAccountNumber")
                    .HasMaxLength(20);

                r.Property(x => x.DateOfBirth)
                    .HasColumnName("RecipientDateOfBirth");

                r.Property(x => x.FirstName)
                    .HasColumnName("RecipientFirstName")
                    .HasMaxLength(250);

                r.Property(x => x.LastName)
                    .HasColumnName("RecipientLastName")
                    .HasMaxLength(250);

                r.Property(x => x.ZipCode)
                    .HasColumnName("RecipientZipCode")
                    .HasMaxLength(20);
            });

            var paymentRecipient = builder.Metadata.FindNavigation(nameof(Payment.Recipient));
            paymentRecipient.SetPropertyAccessMode(PropertyAccessMode.Field);

            builder.OwnsOne(x => x.MerchantNotification, r =>
            {
                r.Property(x => x.SuccessUrl)
                    .HasColumnName("SuccessWebHookUrl");

                r.Property(x => x.ErrorUrl)
                    .HasColumnName("ErrorWebHookUrl");
            });

            var merchantNotificationNavigation = builder.Metadata.FindNavigation(nameof(Payment.MerchantNotification));
            merchantNotificationNavigation.SetPropertyAccessMode(PropertyAccessMode.Field);

            builder.OwnsMany<MetaData>(x => x.MetaData, r =>
            {
                r.ToTable("PaymentMetaData");

                r.Property(x => x.Key)
                    .HasColumnName("Key");

                r.Property(x => x.Value)
                    .HasColumnName("Value");
            });

            var metaDataNavigation = builder.Metadata.FindNavigation(nameof(Payment.MetaData));
            metaDataNavigation.SetField("_metaData");
        }
    }
}
