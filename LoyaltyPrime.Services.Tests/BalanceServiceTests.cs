using System.Threading;
using System.Threading.Tasks;
using LoyaltyPrime.DataAccessLayer;
using LoyaltyPrime.DataAccessLayer.Repositories;
using LoyaltyPrime.DataAccessLayer.Shared.Utilities.Common.Data;
using LoyaltyPrime.DataAccessLayer.Specifications;
using LoyaltyPrime.Models;
using LoyaltyPrime.Models.Bases.Enums;
using LoyaltyPrime.Services.Contexts.AccountRewardHistoryServices.Notifications;
using LoyaltyPrime.Services.Contexts.BalanceManagementServices.Commands;
using MediatR;
using Moq;
using Xunit;

namespace LoyaltyPrime.Services.Tests
{
    public class BalanceServiceTests
    {
        private readonly Mock<IMediator> _mediatorMock = new Mock<IMediator>();

        private readonly Mock<IUnitOfWork> _unitOfWorkMock = new Mock<IUnitOfWork>();

        [Fact]
        public async Task
            CollectPoints_ShouldAddPointToAccountAndCreateAccountRewardHistory_IfMemberAndAccountAndCompanyRewardExists()
        {
            //Arrange
            var account = new Account(1, 1, 100, AccountState.Active);

            var companyReward = new CompanyReward("Free Cofee", 1, 50) {Id = 1};


            Mock<IRepository<CompanyReward>> companyRewardRepositoryMock = new Mock<IRepository<CompanyReward>>();

            Mock<IRepository<Account>> accountRepositoryMock = new Mock<IRepository<Account>>();

            companyRewardRepositoryMock.Setup(s =>
                    s.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(companyReward)
                .Verifiable();

            accountRepositoryMock.Setup(s =>
                    s.FirstOrDefaultAsync(It.IsAny<ISpecification<Account>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(account)
                .Verifiable();

            accountRepositoryMock.Setup(s => s.Update(account))
                .Verifiable();

            _unitOfWorkMock.Setup(s => s.CompanyRewardRepository)
                .Returns(companyRewardRepositoryMock.Object)
                .Verifiable();

            _unitOfWorkMock.Setup(s => s.AccountRepository)
                .Returns(accountRepositoryMock.Object)
                .Verifiable();
            _unitOfWorkMock.Setup(s => s.CommitAsync(It.IsAny<CancellationToken>()))
                .Verifiable();

            var accountRewardHistoryNotification =
                new PlaceAccountRewardHistoryNotification(companyReward.Id, account.Id, companyReward.GainedPoints);

            _mediatorMock.Setup(s => s.Publish(accountRewardHistoryNotification, It.IsAny<CancellationToken>()))
                .Verifiable();

            CollectPointCommand command = new CollectPointCommand();

            CollectPointCommandHandler sut =
                new CollectPointCommandHandler(_unitOfWorkMock.Object, _mediatorMock.Object);

            //Act

            var result = await sut.Handle(command, It.IsAny<CancellationToken>());

            //Assert

            _unitOfWorkMock.Verify(v => v.CompanyRewardRepository);

            _unitOfWorkMock.Verify(v => v.AccountRepository);

            companyRewardRepositoryMock.Verify(v =>
                v.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()));

            accountRepositoryMock.Verify(v =>
                v.FirstOrDefaultAsync(It.IsAny<ISpecification<Account>>(), It.IsAny<CancellationToken>()));

            accountRepositoryMock.Verify(v =>
                v.Update(It.IsAny<Account>()));

            _unitOfWorkMock.Verify(s => s.CommitAsync(It.IsAny<CancellationToken>()));

            _mediatorMock.Verify(f => f.Publish(It.IsAny<PlaceAccountRewardHistoryNotification>(),
                It.IsAny<CancellationToken>()));

            Assert.True(result.IsSucceeded);
            Assert.Equal(150, result.Result);
        }
    }
}