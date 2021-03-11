using System.Threading;
using System.Threading.Tasks;
using LoyaltyPrime.DataAccessLayer;
using LoyaltyPrime.DataAccessLayer.Repositories;
using LoyaltyPrime.Models;
using LoyaltyPrime.Services.Contexts.AccountRewardHistoryServices.Command;
using Moq;
using Xunit;

namespace LoyaltyPrime.Services.Tests
{
    public class AccountRewardServicesHistoryTests
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

            CreateAccountRewardHistoryCommand command =
                new CreateAccountRewardHistoryCommand(rewardHistory.CompanyRewardId, rewardHistory.AccountId,
                    rewardHistory.RewardPoints);

            CreateAccountRewardHistoryCommandHandler sut =
                new CreateAccountRewardHistoryCommandHandler(_unitOfWorkMock.Object);

            //Act
            var result = await sut.Handle(command, It.IsAny<CancellationToken>());

            //Assert

            _unitOfWorkMock.Verify(v => v.AccountRewardHistoryRepository);

            _accountRewardHistoryRepositoryMock.Verify(v =>
                v.AddAsync(It.IsAny<AccountRewardHistory>(), It.IsAny<CancellationToken>()));

            _unitOfWorkMock.Verify(v => v.CommitAsync(It.IsAny<CancellationToken>()));

            Assert.True(result.IsSucceeded && result.StatusCode == 201);
        }
    }
}