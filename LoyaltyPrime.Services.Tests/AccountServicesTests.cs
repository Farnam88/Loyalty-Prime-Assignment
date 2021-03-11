using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using LoyaltyPrime.DataAccessLayer;
using LoyaltyPrime.DataAccessLayer.Repositories;
using LoyaltyPrime.DataAccessLayer.Specifications;
using LoyaltyPrime.Models;
using LoyaltyPrime.Models.Bases.Enums;
using LoyaltyPrime.Services.Contexts.AccountServices.Commands;
using LoyaltyPrime.Services.Contexts.AccountServices.Dto;
using LoyaltyPrime.Services.Contexts.AccountServices.Queries;
using Moq;
using Xunit;

namespace LoyaltyPrime.Services.Tests
{
    public class AccountServicesTests
    {
        private Mock<IRepository<Account>> accountRepositoryMock = new Mock<IRepository<Account>>();

        private readonly Mock<IUnitOfWork> _unitOfWorkMock = new Mock<IUnitOfWork>();

        [Fact]
        public async Task CreateAccount_ShouldCreateAccountForAnSpecificUser_IfCompanyAndMemberExists()
        {
            //Arrange
            var company = new Company("Burger King") {Id = 1};
            var member = new Member("Farnam") {Id = 1};
            var account = new Account(member.Id, company.Id, 0, AccountState.Active);
            Mock<IRepository<Company>> companyRepositoryMock = new Mock<IRepository<Company>>();
            Mock<IRepository<Member>> memberRepositoryMock = new Mock<IRepository<Member>>();

            companyRepositoryMock.Setup(s =>
                    s.FirstOrDefaultAsync(It.IsAny<ISpecification<Company>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(company).Verifiable();

            memberRepositoryMock.Setup(s =>
                    s.FirstOrDefaultAsync(It.IsAny<ISpecification<Member>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(member).Verifiable();

            accountRepositoryMock.Setup(s =>
                s.AddAsync(account, It.IsAny<CancellationToken>())).Verifiable();

            _unitOfWorkMock.Setup(s => s.AccountRepository).Returns(accountRepositoryMock.Object);
            _unitOfWorkMock.Setup(s => s.CompanyRepository).Returns(companyRepositoryMock.Object);
            _unitOfWorkMock.Setup(s => s.MemberRepository).Returns(memberRepositoryMock.Object);

            CreateAccountCommand command = new CreateAccountCommand(member.Id, company.Id);
            CreateAccountCommandHandler sut = new CreateAccountCommandHandler(_unitOfWorkMock.Object);
            //Act
            var result = await sut.Handle(command, It.IsAny<CancellationToken>());

            //Assert
            _unitOfWorkMock.Verify(v => v.MemberRepository);
            _unitOfWorkMock.Verify(v => v.AccountRepository);
            _unitOfWorkMock.Verify(v => v.CompanyRepository);
            companyRepositoryMock.Verify(v =>
                v.FirstOrDefaultAsync(It.IsAny<ISpecification<Company>>(), It.IsAny<CancellationToken>()));
            memberRepositoryMock.Verify(v =>
                v.FirstOrDefaultAsync(It.IsAny<ISpecification<Member>>(), It.IsAny<CancellationToken>()));
            accountRepositoryMock.Verify(v => v.AddAsync(It.IsAny<Account>(), It.IsAny<CancellationToken>()));
            _unitOfWorkMock.Verify(v => v.CommitAsync(It.IsAny<CancellationToken>()));

            Assert.True(result.IsSucceeded);
        }

        [Fact]
        public async Task CreateAccount_ShouldFail_IfCompanyNotExists()
        {
            //Arrange

            var member = new Member("Farnam") {Id = 1};
            Mock<IRepository<Company>> companyRepositoryMock = new Mock<IRepository<Company>>();
            Mock<IRepository<Member>> memberRepositoryMock = new Mock<IRepository<Member>>();

            companyRepositoryMock.Setup(s =>
                    s.FirstOrDefaultAsync(It.IsAny<ISpecification<Company>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Company) null).Verifiable();

            memberRepositoryMock.Setup(s =>
                    s.FirstOrDefaultAsync(It.IsAny<ISpecification<Member>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(member).Verifiable();

            _unitOfWorkMock.Setup(s => s.CompanyRepository).Returns(companyRepositoryMock.Object);
            _unitOfWorkMock.Setup(s => s.MemberRepository).Returns(memberRepositoryMock.Object);

            CreateAccountCommand command = new CreateAccountCommand(It.IsAny<int>(), It.IsAny<int>());
            CreateAccountCommandHandler sut = new CreateAccountCommandHandler(_unitOfWorkMock.Object);
            //Act
            var result = await sut.Handle(command, It.IsAny<CancellationToken>());

            //Assert
            _unitOfWorkMock.Verify(v => v.CompanyRepository);
            companyRepositoryMock.Verify(v =>
                v.FirstOrDefaultAsync(It.IsAny<ISpecification<Company>>(), It.IsAny<CancellationToken>()));

            Assert.False(result.IsSucceeded);
        }

        [Fact]
        public async Task CreateAccount_ShouldFail_IfMemberNotExists()
        {
            //Arrange

            var company = new Company("Burger King") {Id = 1};
            Mock<IRepository<Company>> companyRepositoryMock = new Mock<IRepository<Company>>();
            Mock<IRepository<Member>> memberRepositoryMock = new Mock<IRepository<Member>>();

            companyRepositoryMock.Setup(s =>
                    s.FirstOrDefaultAsync(It.IsAny<ISpecification<Company>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(company).Verifiable();

            memberRepositoryMock.Setup(s =>
                    s.FirstOrDefaultAsync(It.IsAny<ISpecification<Member>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Member) null).Verifiable();

            _unitOfWorkMock.Setup(s => s.CompanyRepository).Returns(companyRepositoryMock.Object);
            _unitOfWorkMock.Setup(s => s.MemberRepository).Returns(memberRepositoryMock.Object);

            CreateAccountCommand command = new CreateAccountCommand(It.IsAny<int>(), It.IsAny<int>());
            CreateAccountCommandHandler sut = new CreateAccountCommandHandler(_unitOfWorkMock.Object);
            //Act
            var result = await sut.Handle(command, It.IsAny<CancellationToken>());

            //Assert
            _unitOfWorkMock.Verify(v => v.MemberRepository);
            memberRepositoryMock.Verify(v =>
                v.FirstOrDefaultAsync(It.IsAny<ISpecification<Member>>(), It.IsAny<CancellationToken>()));

            Assert.False(result.IsSucceeded);
        }

        [Fact]
        public async Task GetMemberActiveAccounts_ShouldReturnSetOfActiveAccount_IfMemberAndAccountExists()
        {
            //Arrange
            int memberId = 1;
            var member = new Member("Farnam") {Id = memberId};
            var activeAccounts = CreateDtoSet();

            accountRepositoryMock.Setup(s =>
                    s.GetAllAsync(It.IsAny<ISpecification<Account, MemberAccountsDto>>(),
                        It.IsAny<CancellationToken>()))
                .ReturnsAsync(activeAccounts).Verifiable();

            Mock<IRepository<Member>> memberRepositoryMock = new Mock<IRepository<Member>>();
            memberRepositoryMock.Setup(s =>
                    s.FirstOrDefaultAsync(It.IsAny<ISpecification<Member>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(member).Verifiable();

            _unitOfWorkMock.Setup(s => s.AccountRepository).Returns(accountRepositoryMock.Object);
            _unitOfWorkMock.Setup(s => s.MemberRepository).Returns(memberRepositoryMock.Object);

            GetMemberActiveAccountsQuery query = new GetMemberActiveAccountsQuery(memberId);

            GetMemberActiveAccountsQueryHandler sut = new GetMemberActiveAccountsQueryHandler(_unitOfWorkMock.Object);

            //Act

            var result = await sut.Handle(query, It.IsAny<CancellationToken>());

            //Assert

            _unitOfWorkMock.Verify(v => v.MemberRepository);

            memberRepositoryMock.Verify(v =>
                v.FirstOrDefaultAsync(It.IsAny<ISpecification<Member>>(), It.IsAny<CancellationToken>()));

            accountRepositoryMock.Verify(v =>
                v.GetAllAsync(It.IsAny<ISpecification<Account, MemberAccountsDto>>(),
                    It.IsAny<CancellationToken>()));

            Assert.True(result.IsSucceeded && result.StatusCode == 200);

            Assert.NotNull(result.Result);
        }

        [Fact]
        public async Task GetAccountById_ShouldReturnAccount_IfAccountExists()
        {
            //Arrange
            var account = new MemberAccountsDto(1, 1, 1, "Burger King",
                "Farnam", 100, AccountState.Active.ToString());

            accountRepositoryMock.Setup(s =>
                    s.FirstOrDefaultAsync(It.IsAny<ISpecification<Account, MemberAccountsDto>>(),
                        It.IsAny<CancellationToken>()))
                .ReturnsAsync(account).Verifiable();

            _unitOfWorkMock.Setup(s => s.AccountRepository).Returns(accountRepositoryMock.Object);

            GetMemberAccountQuery query = new GetMemberAccountQuery(1, 1);
            GetAccountByIdQueryHandler sut = new GetAccountByIdQueryHandler(_unitOfWorkMock.Object);

            //Act
            var result = await sut.Handle(query, It.IsAny<CancellationToken>());

            //Assert
            _unitOfWorkMock.Verify(v => v.AccountRepository);
            accountRepositoryMock.Verify(s =>
                s.FirstOrDefaultAsync(It.IsAny<ISpecification<Account, MemberAccountsDto>>(),
                    It.IsAny<CancellationToken>()));

            Assert.True(result.IsSucceeded && result.StatusCode == 200);

            Assert.NotNull(result.Result);
        }

        private List<MemberAccountsDto> CreateDtoSet()
        {
            return new List<MemberAccountsDto>
            {
                new MemberAccountsDto
                {
                    Balance = 100,
                    State = AccountState.Active.ToString(),
                    AccountId = 1,
                    CompanyId = 1,
                    CompanyName = "Burger King"
                }
            };
        }
    }
}