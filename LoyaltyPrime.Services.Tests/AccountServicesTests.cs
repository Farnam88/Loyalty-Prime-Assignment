using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using LoyaltyPrime.DataAccessLayer;
using LoyaltyPrime.DataAccessLayer.Repositories;
using LoyaltyPrime.DataAccessLayer.Specifications;
using LoyaltyPrime.Models;
using LoyaltyPrime.Models.Bases.Enums;
using LoyaltyPrime.Services.Common.Specifications.AccountSpec;
using LoyaltyPrime.Services.Common.Specifications.MemberSpec;
using LoyaltyPrime.Services.Contexts.AccountServices.Commands;
using LoyaltyPrime.Services.Contexts.AccountServices.Dto;
using LoyaltyPrime.Services.Contexts.AccountServices.Queries;
using Moq;
using Xunit;

namespace LoyaltyPrime.Services.Tests
{
    public class AccountServicesTests
    {
        private readonly Mock<IRepository<Account>> _accountRepositoryMock = new Mock<IRepository<Account>>();

        private readonly Mock<IUnitOfWork> _unitOfWorkMock = new Mock<IUnitOfWork>();

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task CreateAccount_ShouldCreateAccountForAnSpecificUser_IfCompanyAndMemberExists(
            bool existingAccount)
        {
            //Arrange
            var company = new Company("Burger King") {Id = 1};
            var member = new Member("Farnam") {Id = 1};
            var account = new Account(member.Id, company.Id, 0, AccountStatus.Active);

            Mock<IRepository<Company>> companyRepositoryMock = new Mock<IRepository<Company>>();
            Mock<IRepository<Member>> memberRepositoryMock = new Mock<IRepository<Member>>();

            companyRepositoryMock.Setup(s =>
                    s.GetByIdAsync(company.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(company)
                .Verifiable();

            memberRepositoryMock.Setup(s =>
                    s.GetByIdAsync(member.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(member)
                .Verifiable();

            _accountRepositoryMock.Setup(s =>
                    s.FirstOrDefaultAsync(It.IsAny<AccountEntitySpecification>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingAccount ? account : (Account) null)
                .Verifiable();

            _accountRepositoryMock.Setup(s =>
                    s.AddAsync(account, It.IsAny<CancellationToken>()))
                .Verifiable();

            _unitOfWorkMock.Setup(s => s.AccountRepository).Returns(_accountRepositoryMock.Object)
                .Verifiable();

            _unitOfWorkMock.Setup(s => s.CompanyRepository).Returns(companyRepositoryMock.Object)
                .Verifiable();

            _unitOfWorkMock.Setup(s => s.MemberRepository).Returns(memberRepositoryMock.Object)
                .Verifiable();

            _unitOfWorkMock.Setup(s => s.CommitAsync(It.IsAny<CancellationToken>()))
                .Verifiable();

            CreateAccountCommand command = new CreateAccountCommand(member.Id, company.Id);

            CreateAccountCommandHandler sut = new CreateAccountCommandHandler(_unitOfWorkMock.Object);

            //Act
            var result = await sut.Handle(command, It.IsAny<CancellationToken>());

            //Assert

            _unitOfWorkMock.Verify(v => v.MemberRepository);

            _unitOfWorkMock.Verify(v => v.AccountRepository);

            _unitOfWorkMock.Verify(v => v.CompanyRepository);

            companyRepositoryMock.Verify(v =>
                v.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()));

            memberRepositoryMock.Verify(v =>
                v.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()));
            if (existingAccount)
            {
                _accountRepositoryMock.Verify(s =>
                    s.FirstOrDefaultAsync(It.IsAny<AccountEntitySpecification>(), It.IsAny<CancellationToken>()));
                Assert.False(result.IsSucceeded);
                Assert.Equal(404, result.StatusCode);
            }

            if (!existingAccount)
            {
                _accountRepositoryMock.Verify(s =>
                    s.FirstOrDefaultAsync(It.IsAny<AccountEntitySpecification>(), It.IsAny<CancellationToken>()));

                _accountRepositoryMock.Verify(v =>
                    v.AddAsync(It.IsAny<Account>(), It.IsAny<CancellationToken>()));

                _unitOfWorkMock.Verify(v => v.CommitAsync(It.IsAny<CancellationToken>()));

                Assert.True(result.IsSucceeded);
                Assert.Equal(201, result.StatusCode);
            }
        }

        [Theory]
        [InlineData(1, 1, true, false)]
        [InlineData(1, 1, false, false)]
        public async Task CreateAccount_ShouldFail_IfCompanyOrAccountNotExists(int companyId,
            int memberId, bool memberIsNull, bool expectedResult)
        {
            //Arrange
            var company = new Company("Burger King") {Id = 1};

            var member = new Member("Farnam") {Id = 1};

            Mock<IRepository<Company>> companyRepositoryMock = new Mock<IRepository<Company>>();

            Mock<IRepository<Member>> memberRepositoryMock = new Mock<IRepository<Member>>();

            companyRepositoryMock.Setup(s =>
                    s.GetByIdAsync(companyId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(memberIsNull ? company : (Company) null)
                .Verifiable();

            memberRepositoryMock.Setup(s =>
                    s.GetByIdAsync(memberId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(memberIsNull ? (Member) null : member)
                .Verifiable();

            _unitOfWorkMock.Setup(s => s.CompanyRepository).Returns(companyRepositoryMock.Object);

            _unitOfWorkMock.Setup(s => s.MemberRepository).Returns(memberRepositoryMock.Object);

            CreateAccountCommand command = new CreateAccountCommand(companyId, memberId);

            CreateAccountCommandHandler sut = new CreateAccountCommandHandler(_unitOfWorkMock.Object);

            //Act
            var result = await sut.Handle(command, It.IsAny<CancellationToken>());

            //Assert

            _unitOfWorkMock.Verify(v => v.CompanyRepository);
            companyRepositoryMock.Verify(v =>
                v.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()));

            if (memberIsNull)
            {
                _unitOfWorkMock.Verify(v => v.MemberRepository);
                memberRepositoryMock.Verify(v =>
                    v.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()));
            }

            Assert.Equal(expectedResult, result.IsSucceeded);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task GetMemberAccounts_ShouldReturnSetOfAccount_IfMemberAndAccountExists(
            bool withResultSet)
        {
            //Arrange
            int memberId = 1;

            var member = new Member("Farnam") {Id = memberId};

            var activeAccounts = CreateDtoSet();

            var emptySet = new List<AccountDto>();

            _accountRepositoryMock.Setup(s =>
                    s.GetAllAsync(It.IsAny<AccountsDtoSpecification>(),
                        It.IsAny<CancellationToken>()))
                .ReturnsAsync(withResultSet ? activeAccounts : emptySet).Verifiable();

            Mock<IRepository<Member>> memberRepositoryMock = new Mock<IRepository<Member>>();

            memberRepositoryMock.Setup(s =>
                    s.FirstOrDefaultAsync(It.IsAny<FindMemberSpecification>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(member).Verifiable();

            _unitOfWorkMock.Setup(s => s.AccountRepository).Returns(_accountRepositoryMock.Object);

            _unitOfWorkMock.Setup(s => s.MemberRepository).Returns(memberRepositoryMock.Object);

            GetMemberAccountsQuery query = new GetMemberAccountsQuery(memberId);

            GetMemberAccountsQueryHandler sut = new GetMemberAccountsQueryHandler(_unitOfWorkMock.Object);

            //Act

            var result = await sut.Handle(query, It.IsAny<CancellationToken>());

            //Assert

            _unitOfWorkMock.Verify(v => v.MemberRepository);

            memberRepositoryMock.Verify(v =>
                v.FirstOrDefaultAsync(It.IsAny<FindMemberSpecification>(), It.IsAny<CancellationToken>()));

            _accountRepositoryMock.Verify(v =>
                v.GetAllAsync(It.IsAny<AccountsDtoSpecification>(),
                    It.IsAny<CancellationToken>()));

            Assert.True(result.IsSucceeded);

            if (withResultSet)
                Assert.True(result.StatusCode == 200 && result.Result != null);
            if (!withResultSet)
                Assert.True(result.StatusCode == 204 && result.Result == null);
        }

        [Fact]
        public async Task GetAccountById_ShouldReturnAccount_IfAccountExists()
        {
            //Arrange
            var account = new AccountDto(1, 1, 1, "Burger King",
                "Farnam", 100, AccountStatus.Active.ToString());

            _accountRepositoryMock.Setup(s =>
                    s.FirstOrDefaultAsync(It.IsAny<AccountsDtoSpecification>(),
                        It.IsAny<CancellationToken>()))
                .ReturnsAsync(account)
                .Verifiable();

            _unitOfWorkMock.Setup(s => s.AccountRepository).Returns(_accountRepositoryMock.Object);

            GetMemberAccountQuery query = new GetMemberAccountQuery(account.MemberId, account.AccountId);

            GetAccountByIdQueryHandler sut = new GetAccountByIdQueryHandler(_unitOfWorkMock.Object);

            //Act
            var result = await sut.Handle(query, It.IsAny<CancellationToken>());

            //Assert
            _unitOfWorkMock.Verify(v => v.AccountRepository);

            _accountRepositoryMock.Verify(s =>
                s.FirstOrDefaultAsync(It.IsAny<AccountsDtoSpecification>(),
                    It.IsAny<CancellationToken>()));

            Assert.True(result.IsSucceeded && result.StatusCode == 200);

            Assert.NotNull(result.Result);
        }

        private List<AccountDto> CreateDtoSet()
        {
            return new List<AccountDto>
            {
                new AccountDto
                {
                    Balance = 100,
                    Status = AccountStatus.Active.ToString(),
                    AccountId = 1,
                    CompanyId = 1,
                    CompanyName = "Burger King"
                }
            };
        }
    }
}