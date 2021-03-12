using System.Threading;
using System.Threading.Tasks;
using LoyaltyPrime.DataAccessLayer;
using LoyaltyPrime.DataAccessLayer.Repositories;
using LoyaltyPrime.Models;
using LoyaltyPrime.Services.Contexts.AccountRedeemHistoryServices.Notifications;
using Moq;
using Xunit;

namespace LoyaltyPrime.Services.Tests
{
    public class AccountRedeemServicesHistoryTests
    {
        private readonly Mock<IRepository<AccountRedeemHistory>> _accountRedeemHistoryRepositoryMock =
            new Mock<IRepository<AccountRedeemHistory>>();

        private readonly Mock<IUnitOfWork> _unitOfWorkMock = new Mock<IUnitOfWork>();

        [Fact]
        public async Task CreateAccountRedeemHistory_ShouldCreateAsLog_OnSuccess()
        {
            //Arrange
            var redeemHistory = new AccountRedeemHistory(1, 1, 50) {Id = 1};

            _accountRedeemHistoryRepositoryMock.Setup(s =>
                    s.AddAsync(redeemHistory, It.IsAny<CancellationToken>()))
                .Verifiable();

            _unitOfWorkMock.Setup(s => s.AccountRedeemHistoryRepository)
                .Returns(_accountRedeemHistoryRepositoryMock.Object)
                .Verifiable();

            _unitOfWorkMock.Setup(s => s.AccountRedeemHistoryRepository)
                .Returns(_accountRedeemHistoryRepositoryMock.Object)
                .Verifiable();

            _unitOfWorkMock.Setup(s => s.CommitAsync(It.IsAny<CancellationToken>()))
                .Verifiable();
            
            PlaceAccountRedeemHistoryNotification notification =
                new PlaceAccountRedeemHistoryNotification(redeemHistory.CompanyRedeemId, redeemHistory.AccountId,
                    redeemHistory.RedeemPoints);

            PlaceAccountRedeemHistoryNotificationHandler sut =
                new PlaceAccountRedeemHistoryNotificationHandler(_unitOfWorkMock.Object);

            //Act
            await sut.Handle(notification, It.IsAny<CancellationToken>());

            //Assert

            _unitOfWorkMock.Verify(v => v.AccountRedeemHistoryRepository);

            _accountRedeemHistoryRepositoryMock.Verify(v =>
                v.AddAsync(It.IsAny<AccountRedeemHistory>(), It.IsAny<CancellationToken>()));

            _unitOfWorkMock.Verify(v => v.CommitAsync(It.IsAny<CancellationToken>()));
        }
    }
}