using System.Threading;
using System.Threading.Tasks;
using LoyaltyPrime.DataAccessLayer;
using LoyaltyPrime.DataAccessLayer.Repositories;
using LoyaltyPrime.Models;
using LoyaltyPrime.Services.Contexts.AccountRedeemHistoryServices.Command;
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
            var RedeemHistory = new AccountRedeemHistory(1, 1, 50) {Id = 1};

            _accountRedeemHistoryRepositoryMock.Setup(s =>
                    s.AddAsync(RedeemHistory, It.IsAny<CancellationToken>()))
                .Verifiable();

            _unitOfWorkMock.Setup(s => s.AccountRedeemHistoryRepository)
                .Returns(_accountRedeemHistoryRepositoryMock.Object)
                .Verifiable();

            _unitOfWorkMock.Setup(s => s.AccountRedeemHistoryRepository)
                .Returns(_accountRedeemHistoryRepositoryMock.Object)
                .Verifiable();

            CreateAccountRedeemHistoryCommand command =
                new CreateAccountRedeemHistoryCommand(RedeemHistory.CompanyRedeemId, RedeemHistory.AccountId,
                    RedeemHistory.RedeemPoints);

            CreateAccountRedeemHistoryCommandHandler sut =
                new CreateAccountRedeemHistoryCommandHandler(_unitOfWorkMock.Object);

            //Act
            var result = await sut.Handle(command, It.IsAny<CancellationToken>());

            //Assert

            _unitOfWorkMock.Verify(v => v.AccountRedeemHistoryRepository);

            _accountRedeemHistoryRepositoryMock.Verify(v =>
                v.AddAsync(It.IsAny<AccountRedeemHistory>(), It.IsAny<CancellationToken>()));

            _unitOfWorkMock.Verify(v => v.CommitAsync(It.IsAny<CancellationToken>()));

            Assert.True(result.IsSucceeded && result.StatusCode == 201);
        }
    }
}