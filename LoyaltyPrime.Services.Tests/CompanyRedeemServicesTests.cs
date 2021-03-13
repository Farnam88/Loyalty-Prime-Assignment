using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using LoyaltyPrime.DataAccessLayer;
using LoyaltyPrime.DataAccessLayer.Repositories;
using LoyaltyPrime.DataAccessLayer.Specifications;
using LoyaltyPrime.Models;
using LoyaltyPrime.Services.Common.Specifications.CompanyRedeemSpec;
using LoyaltyPrime.Services.Contexts.CompanyRedeemServices.Commands;
using LoyaltyPrime.Services.Contexts.CompanyRedeemServices.Dto;
using LoyaltyPrime.Services.Contexts.CompanyRedeemServices.Queries;
using Moq;
using Xunit;

namespace LoyaltyPrime.Services.Tests
{
    public class CompanyRedeemServicesTests
    {
        private readonly Mock<IRepository<CompanyRedeem>> _companyRedeemRepositoryMock =
            new Mock<IRepository<CompanyRedeem>>();

        private readonly Mock<IUnitOfWork> _unitOfWorkMock = new Mock<IUnitOfWork>();

        [Fact]
        public async Task CreateCompanyRedeem_ShouldCreateRedeem_IfCompanyExists()
        {
            //Arrange
            var company = new Company("Burger King") {Id = 1};

            var redeem = new CompanyRedeem("Free Coffee", company.Id, 50) {Id = 1};

            Mock<IRepository<Company>> companyRepositoryMock = new Mock<IRepository<Company>>();

            companyRepositoryMock.Setup(s =>
                    s.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(company).Verifiable();

            _companyRedeemRepositoryMock.Setup(s =>
                    s.AddAsync(redeem, It.IsAny<CancellationToken>()))
                .Verifiable();

            _unitOfWorkMock.Setup(s => s.CompanyRedeemRepository)
                .Returns(_companyRedeemRepositoryMock.Object)
                .Verifiable();

            _unitOfWorkMock.Setup(s => s.CompanyRepository)
                .Returns(companyRepositoryMock.Object)
                .Verifiable();

            _unitOfWorkMock.Setup(v => v.CommitAsync(It.IsAny<CancellationToken>()))
                .Verifiable();


            CreateCompanyRedeemCommand command = new CreateCompanyRedeemCommand(redeem.CompanyId,
                redeem.RedeemTitle, redeem.RedeemPoints);

            CreateCompanyRedeemCommandHandler sut =
                new CreateCompanyRedeemCommandHandler(_unitOfWorkMock.Object);

            //Act
            var result = await sut.Handle(command, It.IsAny<CancellationToken>());

            //Assert
            _unitOfWorkMock.Verify(v => v.CompanyRedeemRepository);

            _unitOfWorkMock.Verify(v => v.CompanyRepository);

            companyRepositoryMock.Verify(v =>
                v.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()));

            _companyRedeemRepositoryMock.Verify(v =>
                v.AddAsync(It.IsAny<CompanyRedeem>(), It.IsAny<CancellationToken>()));

            _unitOfWorkMock.Verify(v => v.CommitAsync(It.IsAny<CancellationToken>()));

            Assert.True(result.IsSucceeded && result.StatusCode == 201);
        }


        [Fact]
        public async Task GetMemberActiveAccounts_ShouldReturnSetOfActiveAccount_IfMemberAndAccountExists()
        {
            //Arrange
            var company = new Company("Burger King") {Id = 1};

            List<CompanyRedeemDto> companyRedeemDto = CompanyRedeemsDto(company.Id, company.Name);

            Mock<IRepository<Company>> companyRepositoryMock = new Mock<IRepository<Company>>();

            _companyRedeemRepositoryMock.Setup(s =>
                    s.GetAllAsync(It.IsAny<CompanyRedeemsSpecification>(),
                        It.IsAny<CancellationToken>()))
                .ReturnsAsync(companyRedeemDto)
                .Verifiable();

            companyRepositoryMock.Setup(s =>
                    s.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(company)
                .Verifiable();

            _unitOfWorkMock.Setup(s => s.CompanyRepository).Returns(companyRepositoryMock.Object);

            _unitOfWorkMock.Setup(s => s.CompanyRedeemRepository).Returns(_companyRedeemRepositoryMock.Object);

            GetCompanyRedeemsQuery query = new GetCompanyRedeemsQuery(company.Id);

            GetCompanyRedeemsQueryHandler sut = new GetCompanyRedeemsQueryHandler(_unitOfWorkMock.Object);

            //Act

            var result = await sut.Handle(query, It.IsAny<CancellationToken>());

            //Assert

            _unitOfWorkMock.Verify(v => v.CompanyRepository);

            _unitOfWorkMock.Verify(v => v.CompanyRedeemRepository);

            companyRepositoryMock.Verify(v =>
                v.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()));

            _companyRedeemRepositoryMock.Verify(v =>
                v.GetAllAsync(It.IsAny<CompanyRedeemsSpecification>(),
                    It.IsAny<CancellationToken>()));

            Assert.True(result.IsSucceeded && result.StatusCode == 200);

            Assert.NotNull(result.Result);
        }

        private List<CompanyRedeemDto> CompanyRedeemsDto(int companyId, string companyName)
        {
            return new List<CompanyRedeemDto>
            {
                new CompanyRedeemDto(companyId, companyName, "Free Coffee", 50, 1)
            };
        }
    }
}