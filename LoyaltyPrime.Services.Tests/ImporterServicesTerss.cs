using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LoyaltyPrime.DataAccessLayer;
using LoyaltyPrime.DataAccessLayer.Repositories;
using LoyaltyPrime.Models;
using LoyaltyPrime.Services.Contexts.ImporterServices.Builder;
using LoyaltyPrime.Services.Contexts.ImporterServices.Commands;
using LoyaltyPrime.Services.Contexts.ImporterServices.Models;
using Moq;
using Xunit;

namespace LoyaltyPrime.Services.Tests
{
    public class ImporterServicesTerss
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock = new Mock<IUnitOfWork>();

        [Fact]
        public async Task Importer_ShouldCreateMemberAndCompanyAndAccountsOfMembers_OnSuccess()
        {
            //Arrange

            var importObjectSet = CreateImportObjectSet();

            var companies = new ImportModelCompanyBuilder(importObjectSet)
                .BuildCompanies()
                .GetCompanies();

            var members = importObjectSet
                .Select(s => new Member(s.Name, s.Address))
                .ToList();

            var accounts = new ImportModelAccountBuilder(companies, members, importObjectSet)
                .BuildAccounts()
                .GetAccounts();

            var memberRepositoryMock = new Mock<IRepository<Member>>();

            var accountRepositoryMock = new Mock<IRepository<Account>>();

            var companyRepositoryMock = new Mock<IRepository<Company>>();

            companyRepositoryMock.Setup(s =>
                    s.AddRangeAsync(companies, It.IsAny<CancellationToken>()))
                .Verifiable();

            memberRepositoryMock.Setup(s =>
                    s.AddRangeAsync(members, It.IsAny<CancellationToken>()))
                .Verifiable();

            accountRepositoryMock.Setup(s =>
                    s.AddRangeAsync(accounts, It.IsAny<CancellationToken>()))
                .Verifiable();

            _unitOfWorkMock.Setup(s => s.CompanyRepository)
                .Returns(companyRepositoryMock.Object)
                .Verifiable();

            _unitOfWorkMock.Setup(s => s.MemberRepository)
                .Returns(memberRepositoryMock.Object)
                .Verifiable();

            _unitOfWorkMock.Setup(s => s.AccountRepository)
                .Returns(accountRepositoryMock.Object)
                .Verifiable();

            _unitOfWorkMock.Setup(s => s.CommitAsync(It.IsAny<CancellationToken>()))
                .Verifiable();

            ImporterCommand command = new ImporterCommand(importObjectSet);

            ImporterCommandHandler sut = new ImporterCommandHandler(_unitOfWorkMock.Object);

            //Act

            var result = await sut.Handle(command, It.IsAny<CancellationToken>());

            //Assert

            companyRepositoryMock.Verify(s =>
                s.AddRangeAsync(It.IsAny<IList<Company>>(), It.IsAny<CancellationToken>()));

            memberRepositoryMock.Verify(s =>
                s.AddRangeAsync(It.IsAny<IList<Member>>(), It.IsAny<CancellationToken>()));

            accountRepositoryMock.Verify(s =>
                s.AddRangeAsync(It.IsAny<IList<Account>>(), It.IsAny<CancellationToken>()));

            _unitOfWorkMock.Verify(s => s.CommitAsync(It.IsAny<CancellationToken>()));

            Assert.True(result.IsSucceeded);
            Assert.True(result.StatusCode == 200);
        }

        public List<ImportModel> CreateImportObjectSet()
        {
            return new List<ImportModel>()
            {
                new ImportModel("Anakin Skywalker", "Landsberger Straße 110", new List<ImportAccountModel>
                {
                    new ImportAccountModel("Burger King", 10, "ACTIVE"),
                    new ImportAccountModel("Fitness First", 150, "INACTIVE")
                }),
                new ImportModel("Yoda", "Landsberger Straße 125", new List<ImportAccountModel>()),
                new ImportModel("Obi-Wan Kenobi", "Landsberger Straße 114", new List<ImportAccountModel>
                {
                    new ImportAccountModel("Burger King", 20, "ACTIVE"),
                    new ImportAccountModel("Fitness First", 17, "ACTIVE"),
                    new ImportAccountModel("Lufthansa", 0, "ACTIVE")
                }),
            };
        }
    }
}