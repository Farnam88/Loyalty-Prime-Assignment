using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using LoyaltyPrime.DataAccessLayer;
using LoyaltyPrime.DataAccessLayer.Repositories;
using LoyaltyPrime.Models;
using LoyaltyPrime.Services.Common.Specifications.CompanySpec;
using LoyaltyPrime.Services.Contexts.CompanyServices.Dtos;
using LoyaltyPrime.Services.Contexts.CompanyServices.Queries;
using Moq;
using Xunit;

namespace LoyaltyPrime.Services.Tests
{
    public class CompanyServicesTest
    {
        private readonly Mock<IRepository<Company>> _companyRepositoryMock = new Mock<IRepository<Company>>();

        private readonly Mock<IUnitOfWork> _unitOfWorkMock = new Mock<IUnitOfWork>();

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task GetCompanies_ShouldReturnSetOfCompanyDto_OnSuccess(bool withResultSet)
        {
            var companyDtoSet = CompanyDtoSet();
            var emptySet = new List<CompanyDto>();
            _companyRepositoryMock.Setup(s =>
                    s.GetAllAsync(It.IsAny<CompanyDtoSpecification>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(withResultSet ? companyDtoSet : emptySet)
                .Verifiable();

            _unitOfWorkMock.Setup(s => s.CompanyRepository)
                .Returns(_companyRepositoryMock.Object)
                .Verifiable();

            GetCompaniesQuery query = new GetCompaniesQuery();

            GetCompaniesQueryHandler sut = new GetCompaniesQueryHandler(_unitOfWorkMock.Object);

            //Act

            var result = await sut.Handle(query, It.IsAny<CancellationToken>());

            //Assert

            _unitOfWorkMock.Verify(v => v.CompanyRepository);

            _companyRepositoryMock.Verify(v =>
                v.GetAllAsync(It.IsAny<CompanyDtoSpecification>(), It.IsAny<CancellationToken>()));

            Assert.True(result.IsSucceeded);
            if (withResultSet)
                Assert.True(result.StatusCode == 200 && result.Result != null);
            if (!withResultSet)
                Assert.True(result.StatusCode == 204 && result.Result == null);
        }

        private List<CompanyDto> CompanyDtoSet()
        {
            return new List<CompanyDto>()
            {
                new CompanyDto(1, "Fitness First"),
                new CompanyDto(2, "Lufthansa")
            };
        }
    }
}