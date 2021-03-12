using System.Threading;
using System.Threading.Tasks;
using LoyaltyPrime.Services.Contexts.BalanceManagementServices.Commands;
using LoyaltyPrime.Shared.Utilities.Common.Data;
using LoyaltyPrime.WebApi.Controllers;
using MediatR;
using Moq;
using Xunit;

namespace LoyaltyPrime.ApiTests
{
    public class BalanceManagementApiTests
    {
        private readonly Mock<IMediator> _mediatorMock = new Mock<IMediator>();

        [Fact]
        public async Task CollectPoints_ShouldIncreaseAccountBalance_IfAccountAndCompanyRewardExists()
        {
            //Arrange

            var command = new CollectPointCommand(1, 1, 1);
            var expectedResult = ResultModel<double>.Success(200, $"50 points added to the account balance",
                150);

            _mediatorMock.Setup(s =>
                    s.Send(It.IsAny<CollectPointCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedResult)
                .Verifiable();

            BalanceManagementController sut = new BalanceManagementController(_mediatorMock.Object);

            //Act
            await sut.CollectPoints(command);

            //Assert

            _mediatorMock.Verify(s =>
                s.Send(It.IsAny<CollectPointCommand>(), It.IsAny<CancellationToken>()));
        }

        [Fact]
        public async Task RedeemPoints_ShouldDecreaseAccountBalance_IfAccountAndCompanyRewardAndAccountIsActiveExists()
        {
            //Arrange

            var command = new RedeemPointCommand(1, 1, 1);
            var expectedResult = ResultModel<double>.Success(200, $"100 points redeemed from the account balance",
                50);

            _mediatorMock.Setup(s =>
                    s.Send(It.IsAny<RedeemPointCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedResult)
                .Verifiable();

            BalanceManagementController sut = new BalanceManagementController(_mediatorMock.Object);

            //Act
            await sut.RedeemPoints(command);

            //Assert

            _mediatorMock.Verify(s =>
                s.Send(It.IsAny<RedeemPointCommand>(), It.IsAny<CancellationToken>()));
        }
    }
}