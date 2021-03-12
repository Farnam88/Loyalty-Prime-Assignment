using System.Threading;
using System.Threading.Tasks;
using LoyaltyPrime.DataAccessLayer;
using LoyaltyPrime.DataAccessLayer.Infrastructure;
using LoyaltyPrime.DataLayer;
using LoyaltyPrime.Infrastructure.Tests.Helpers;
using LoyaltyPrime.Models;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace LoyaltyPrime.Infrastructure.Tests
{
    public class UnitOfWorkTest
    {
        private IUnitOfWork _sut;
        private readonly Mock<LoyaltyPrimeContext> _mockContext = new Mock<LoyaltyPrimeContext>();

        [Fact]
        public void Create_ShouldCreateUnitOfWork_OnSuccess()
        {
            //Arrange

            //Act
            _sut = new UnitOfWork(_mockContext.Object);

            //Assert
            Assert.NotNull(_sut);
        }

        [Fact]
        public void UnitOfWorkRepository_ShouldCreateRepository_OnDemand()
        {
            //Arrange
            var mockCompanyDbSet = new Mock<DbSet<Company>>();
            var mockAccountDbSet = new Mock<DbSet<Account>>();
            var mockMemberDbSet = new Mock<DbSet<Member>>();
            var mockCompanyRedeemDbSet = new Mock<DbSet<CompanyRedeem>>();
            var mockCompanyRewardDbSet = new Mock<DbSet<CompanyReward>>();
            var mockAccountRedeemHistoryDbSet = new Mock<DbSet<AccountRedeemHistory>>();
            var mockAccountRewardHistoryDbSet = new Mock<DbSet<AccountRewardHistory>>();
            _mockContext.Setup(s => s.Set<Company>()).Returns(mockCompanyDbSet.Object);
            _mockContext.Setup(s => s.Set<Account>()).Returns(mockAccountDbSet.Object);
            _mockContext.Setup(s => s.Set<Member>()).Returns(mockMemberDbSet.Object);
            _mockContext.Setup(s => s.Set<CompanyRedeem>()).Returns(mockCompanyRedeemDbSet.Object);
            _mockContext.Setup(s => s.Set<CompanyReward>()).Returns(mockCompanyRewardDbSet.Object);
            _mockContext.Setup(s => s.Set<AccountRedeemHistory>()).Returns(mockAccountRedeemHistoryDbSet.Object);
            _mockContext.Setup(s => s.Set<AccountRewardHistory>()).Returns(mockAccountRewardHistoryDbSet.Object);
            _sut = new UnitOfWork(_mockContext.Object);

            //Act
            var companyRepository = _sut.CompanyRepository;
            var accountRepository = _sut.AccountRepository;
            var memberRepository = _sut.MemberRepository;
            var companyRedeemRepository = _sut.CompanyRedeemRepository;
            var companyRewardRepository = _sut.CompanyRewardRepository;
            var accountRedeemRepository = _sut.AccountRedeemHistoryRepository;
            var accountRewardRepository = _sut.AccountRewardHistoryRepository;
            //Assert
            Assert.NotNull(companyRepository);
            Assert.NotNull(accountRepository);
            Assert.NotNull(memberRepository);
            Assert.NotNull(companyRedeemRepository);
            Assert.NotNull(companyRewardRepository);
            Assert.NotNull(accountRedeemRepository);
            Assert.NotNull(accountRewardRepository);
        }

        [Fact]
        public async Task SaveChange_ShouldSaveEntity_OnSuccess()
        {
            //Arrange
            var company = EntityGenerator.CreateCompany();

            var mockCompanyDbSet = new Mock<DbSet<Company>>();

            mockCompanyDbSet.Setup(s => s.AddAsync(company, It.IsAny<CancellationToken>()))
                .Verifiable();

            _mockContext.Setup(s => s.Set<Company>())
                .Returns(mockCompanyDbSet.Object)
                .Verifiable();

            _mockContext.Setup(s => s.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .Verifiable();

            //Act
            _sut = new UnitOfWork(_mockContext.Object);

            await _sut.CompanyRepository.AddAsync(company);

            await _sut.CommitAsync(It.IsAny<CancellationToken>());

            //Assert
            mockCompanyDbSet.Verify(s => s.AddAsync(It.IsAny<Company>(), It.IsAny<CancellationToken>()));
            
            _mockContext.Verify(s => s.Set<Company>());

            _mockContext.Verify(s => s.SaveChangesAsync(It.IsAny<CancellationToken>()));
        }
    }
}