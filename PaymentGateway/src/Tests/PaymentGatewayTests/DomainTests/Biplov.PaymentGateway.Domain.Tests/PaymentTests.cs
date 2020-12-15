using System;
using AutoFixture.Xunit2;

using Biplov.PaymentGateway.Domain.Entities;
using Biplov.PaymentGateway.Domain.Enum;
using Shouldly;
using Xunit;

namespace Biplov.PaymentGateway.Domain.Tests
{
    public class PaymentTests
    {
        [Theory, AutoData]
        public void InitiatePayment_SetsPaymentStatus_ToInProcess(Guid merchantId, decimal amount, string reference, string description)
        {
            // Arrange
            var payment = new Payment(merchantId, "EUR", amount, reference, "", description);
            payment.SetCardPaymentSource("cardtok_abcasdresadfeadfin123", "251");
            // Act
            payment.InitiatePayment();

            // Assert
            payment.Status.ShouldBe(PaymentStatus.InProcess);
        }

        [Theory, AutoData]
        public void InitiatePayment_SetsPaymentStatus_RaisesDomainEvent(Guid merchantId, decimal amount, string reference, string description)
        {
            // Arrange
            var payment = new Payment(merchantId, "EUR", amount, reference, "", description);
            payment.SetCardPaymentSource("cardtok_abcasdresadfeadfin123", "251");
            // Act
            payment.InitiatePayment();

            // Assert
            payment.DomainEvents.Count.ShouldBe(1);
        }
    }
}
