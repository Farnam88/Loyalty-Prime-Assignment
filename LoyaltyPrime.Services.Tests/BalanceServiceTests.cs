using System.Threading;
using System.Threading.Tasks;
using LoyaltyPrime.DataAccessLayer;
using LoyaltyPrime.DataAccessLayer.Repositories;
using LoyaltyPrime.Models;
using LoyaltyPrime.Models.Bases.Enums;
using LoyaltyPrime.Services.Common.Specifications.AccountSpec;
using LoyaltyPrime.Services.Contexts.AccountRedeemHistoryServices.Notifications;
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
            CollectPoints_ShouldAddPointToAccountAndPublishAccountRewardHistory_IfMemberAndAccountAndCompanyRewardExists()
        {
            //Arrange
            var account = new Account(1, 1, 100, AccountStatus.Active);

            var companyReward = new CompanyReward("International Flight", 1, 50) {Id = 1};


            Mock<IRepository<CompanyReward>> companyRewardRepositoryMock = new Mock<IRepository<CompanyReward>>();

            Mock<IRepository<Account>> accountRepositoryMock = new Mock<IRepository<Account>>();

            companyRewardRepositoryMock.Setup(s =>
                    s.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(companyReward)
                .Verifiable();

            accountRepositoryMock.Setup(s =>
                    s.FirstOrDefaultAsync(It.IsAny<AccountEntitySpecification>(), It.IsAny<CancellationToken>()))
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
                new PlaceAccountRewardHistoryNotification(companyReward.Id, account.Id, companyReward.RewardPoints);

            _mediatorMock.Setup(s => s.Publish(accountRewardHistoryNotification, It.IsAny<CancellationToken>()))
                .Verifiable();

            CollectPointCommand command = new CollectPointCommand(account.Id, account.MemberId, companyReward.Id);

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
                v.FirstOrDefaultAsync(It.IsAny<AccountEntitySpecification>(), It.IsAny<CancellationToken>()));

            accountRepositoryMock.Verify(v =>
                v.Update(It.IsAny<Account>()));

            _unitOfWorkMock.Verify(s => s.CommitAsync(It.IsAny<CancellationToken>()));

            _mediatorMock.Verify(f => f.Publish(It.IsAny<PlaceAccountRewardHistoryNotification>(),
                It.IsAny<CancellationToken>()));

            Assert.True(result.IsSucceeded);
            Assert.True(result.StatusCode==200);
            Assert.Equal(150, result.Result);
        }

        [Fact]
        public async Task
            RedeemPoints_ShouldRedeemPointFromAccountAndPublishAccountRedeemHistory_IfMemberAndAccountAndCompanyRedeemdExists()
        {
            //Arrange
            var account = new Account(1, 1, 100, AccountStatus.Active);

            var companyRedeem = new CompanyRedeem("Free Coffee", 1, 50) {Id = 1};


            Mock<IRepository<CompanyRedeem>> companyRedeemRepositoryMock = new Mock<IRepository<CompanyRedeem>>();

            Mock<IRepository<Account>> accountRepositoryMock = new Mock<IRepository<Account>>();

            companyRedeemRepositoryMock.Setup(s =>
                    s.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(companyRedeem)
                .Verifiable();

            accountRepositoryMock.Setup(s =>
                    s.FirstOrDefaultAsync(It.IsAny<AccountEntitySpecification>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(account)
                .Verifiable();

            accountRepositoryMock.Setup(s => s.Update(account))
                .Verifiable();

            _unitOfWorkMock.Setup(s => s.CompanyRedeemRepository)
                .Returns(companyRedeemRepositoryMock.Object)
                .Verifiable();

            _unitOfWorkMock.Setup(s => s.AccountRepository)
                .Returns(accountRepositoryMock.Object)
                .Verifiable();

            _unitOfWorkMock.Setup(s => s.CommitAsync(It.IsAny<CancellationToken>()))
                .Verifiable();

            var accountRedeemHistoryNotification =
                new PlaceAccountRedeemHistoryNotification(companyRedeem.Id, account.Id, companyRedeem.RedeemPoints);

            _mediatorMock.Setup(s => s.Publish(accountRedeemHistoryNotification, It.IsAny<CancellationToken>()))
                .Verifiable();

            RedeemPointCommand command = new RedeemPointCommand(account.Id,account.MemberId,companyRedeem.Id);

            RedeemPointCommandHandler sut =
                new RedeemPointCommandHandler(_unitOfWorkMock.Object, _mediatorMock.Object);

            //Act

            var result = await sut.Handle(command, It.IsAny<CancellationToken>());

            //Assert

            _unitOfWorkMock.Verify(v => v.CompanyRedeemRepository);

            _unitOfWorkMock.Verify(v => v.AccountRepository);

            companyRedeemRepositoryMock.Verify(v =>
                v.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()));

            accountRepositoryMock.Verify(v =>
                v.FirstOrDefaultAsync(It.IsAny<AccountEntitySpecification>(), It.IsAny<CancellationToken>()));

            accountRepositoryMock.Verify(v =>
                v.Update(It.IsAny<Account>()));

            _unitOfWorkMock.Verify(s => s.CommitAsync(It.IsAny<CancellationToken>()));

            _mediatorMock.Verify(f => f.Publish(It.IsAny<PlaceAccountRedeemHistoryNotification>(),
                It.IsAny<CancellationToken>()));

            Assert.True(result.IsSucceeded);
            Assert.True(result.StatusCode==200);
            Assert.Equal(50, result.Result);
        }
    }
}