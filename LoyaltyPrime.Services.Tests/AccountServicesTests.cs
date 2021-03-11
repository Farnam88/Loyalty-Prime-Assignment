using System.Threading;
using System.Threading.Tasks;
using LoyaltyPrime.DataAccessLayer;
using LoyaltyPrime.DataAccessLayer.Repositories;
using LoyaltyPrime.DataAccessLayer.Specifications;
using LoyaltyPrime.Models;
using LoyaltyPrime.Models.Bases.Enums;
using LoyaltyPrime.Services.Contexts.AccountServices.Commands;
using Moq;
using Xunit;

namespace LoyaltyPrime.Services.Tests
{
    public class AccountServicesTests
    {
        private Mock<IRepository<Account>> repositoryMock = new Mock<IRepository<Account>>();

        private readonly Mock<IUnitOfWork> _unitOfWorkMock = new Mock<IUnitOfWork>();

        [Fact]
        public async Task CreateAccount_ShouldCreateAccountForAnSpecificUser_IfCompanyAndMemberExists()
        {
            //
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

            repositoryMock.Setup(s =>
                s.AddAsync(account, It.IsAny<CancellationToken>())).Verifiable();

            _unitOfWorkMock.Setup(s => s.AccountRepository).Returns(repositoryMock.Object);
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
            repositoryMock.Verify(v => v.AddAsync(It.IsAny<Account>(), It.IsAny<CancellationToken>()));
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
    }
}