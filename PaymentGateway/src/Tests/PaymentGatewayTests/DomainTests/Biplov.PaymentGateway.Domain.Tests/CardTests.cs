using System;
using Biplov.PaymentGateway.Domain.Entities;
using Shouldly;
using Xunit;

namespace Biplov.PaymentGateway.Domain.Tests
{
    public class CardTests
    {

        [Fact]
        public void CardConstructor_GivenInvalidExpiryYear_ShouldThrowArgumentException()
        {
            // Arrange


            // Act
            Action action = () => new Card("111222333444", 12, 2001, "Biplov KC", "");

            // Assert
            action.ShouldThrow<ArgumentException>().Message.ShouldBe("card_expired");
        }

        [Fact]
        public void CardConstructor_GivenNullName_ShouldThrowArgumentNullException()
        {
            // Arrange


            // Act
            Action action = () => new Card("111222333444", 12, 2021, null, "251");


            // Assert
            action.ShouldThrow<ArgumentNullException>().Message.ShouldContain("name");
        }

        [Fact]
        public void CardConstructor_GivenValidData_ShouldReturnCardTokenWithValidPrefix()
        {
            // Arrange


            // Act
           var card =  new Card("111222333444", 12, 2021, "Biplov KC", "251");


            // Assert
            card.CardToken.ShouldContain("cardtok_");
        }

        [Fact]
        public void CardConstructor_GivenValidData_ShouldReturnCardTokenOf40Characters()
        {
            // Arrange


            // Act
            var card =  new Card("111222333444", 12, 2021, "Biplov KC", "251");


            // Assert
            card.CardToken.Length.ShouldBe(40);
        }

        [Fact]
        public void CardConstructor_GivenValidData_ShouldReturnMaskedCardWithLast4NumbersVisible()
        {
            // Arrange


            // Act
            var card =  new Card("1112223334444", 12, 2021, "Biplov KC", "251");


            // Assert
            card.MaskedCardNumber.ShouldBe("*********4444");
        }
    }
}
