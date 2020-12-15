using System;
using System.Threading;
using Biplov.Common.Core;
using Biplov.PaymentGateway.Application.Commands;
using Biplov.PaymentGateway.Application.Queries;
using Biplov.PaymentGateway.Application.Request;
using Biplov.PaymentGatewayApi.Controllers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Shouldly;
using Xunit;

namespace Biplov.PaymentGateway.Api.Tests
{
    public class CardControllerTests
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly Mock<IMerchantQuery> _merchantQueryMock;
        private readonly CardController _sut;

        public CardControllerTests()
        {
            _merchantQueryMock = new Mock<IMerchantQuery>();
            _mediatorMock = new Mock<IMediator>();
            _sut = new CardController(_mediatorMock.Object, _merchantQueryMock.Object);
        }

        [Fact]
        public void TokenizeCard_GivenValidResult_ShouldReturnOk()
        {
            //Arrange
            var cardRequest = new TokenizeCardRequest
            {
                Name = "Biplov KC",
                Number = "111222333444",
                ExpiryYear = 2021,
                ExpiryMonth = 12,
                Cvv = "222",
            };

            _merchantQueryMock
                .Setup(x => x.GetMerchantIdAsync(It.IsNotNull<string>()))
                .ReturnsAsync(It.IsAny<Guid>());

            var innerCommand = new AddNewCardCommand(cardRequest.Name, cardRequest.Number, cardRequest.ExpiryMonth, cardRequest.ExpiryYear, cardRequest.Cvv, null, It.IsAny<string>());
            var command = new IdentifiedCommand<AddNewCardCommand, Result>(innerCommand);
            _mediatorMock.Setup(x => x.Send(command, It.IsAny<CancellationToken>())).ReturnsAsync(Result.Ok);

            //Act
            var result = _sut.TokenizeCard(cardRequest).GetAwaiter().GetResult();
            var okResult = result as OkObjectResult;
            //Assert
            okResult.ShouldNotBeNull();
            okResult.StatusCode.ShouldBe(200);
        }
    }
}
