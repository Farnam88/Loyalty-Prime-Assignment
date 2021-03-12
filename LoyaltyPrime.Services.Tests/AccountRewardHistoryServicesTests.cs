using System.Threading;
using System.Threading.Tasks;
using LoyaltyPrime.DataAccessLayer;
using LoyaltyPrime.DataAccessLayer.Repositories;
using LoyaltyPrime.Models;
using LoyaltyPrime.Services.Contexts.AccountRewardHistoryServices.Notifications;
using Moq;
using Xunit;

namespace LoyaltyPrime.Services.Tests
{
    public class AccountRewardHistoryServicesTests
    {
        private readonly Mock<IRepository<AccountRewardHistory>> _accountRewardHistoryRepositoryMock =
            new Mock<IRepository<AccountRewardHistory>>();

        private readonly Mock<IUnitOfWork> _unitOfWorkMock = new Mock<IUnitOfWork>();

        [Fact]
        public async Task CreateAccountRewardHistory_ShouldCreateAsLog_OnSuccess()
        {
            //Arrange
            var rewardHistory = new AccountRewardHistory(1, 1, 50) {Id = 1};

            _accountRewardHistoryRepositoryMock.Setup(s =>
                    s.AddAsync(rewardHistory, It.IsAny<CancellationToken>()))
                .Verifiable();

            _unitOfWorkMock.Setup(s => s.AccountRewardHistoryRepository)
                .Returns(_accountRewardHistoryRepositoryMock.Object)
                .Verifiable();

            _unitOfWorkMock.Setup(s => s.AccountRewardHistoryRepository)
                .Returns(_accountRewardHistoryRepositoryMock.Object)
                .Verifiable();
            _unitOfWorkMock.Setup(s => s.CommitAsync(It.IsAny<CancellationToken>()))
                .Verifiable();

            PlaceAccountRewardHistoryNotification command =
                new PlaceAccountRewardHistoryNotification(rewardHistory.CompanyRewardId, rewardHistory.AccountId,
                    rewardHistory.RewardPoints);

            PlaceAccountRewardHistoryNotificationHandler sut =
                new PlaceAccountRewardHistoryNotificationHandler(_unitOfWorkMock.Object);

            //Act
            await sut.Handle(command, It.IsAny<CancellationToken>());

            //Assert

            _unitOfWorkMock.Verify(v => v.AccountRewardHistoryRepository);

            _accountRewardHistoryRepositoryMock.Verify(v =>
                v.AddAsync(It.IsAny<AccountRewardHistory>(), It.IsAny<CancellationToken>()));

            _unitOfWorkMock.Verify(v => v.CommitAsync(It.IsAny<CancellationToken>()));
        }
    }
}