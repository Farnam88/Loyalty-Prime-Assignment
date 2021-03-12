using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using LoyaltyPrime.Services.Contexts.CompanyServices.Dtos;
using LoyaltyPrime.Services.Contexts.CompanyServices.Queries;
using LoyaltyPrime.Shared.Utilities.Common.Data;
using LoyaltyPrime.WebApi.Controllers;
using MediatR;
using Moq;
using Xunit;

namespace LoyaltyPrime.ApiTests
{
    public class CompanyApiTests
    {
        private readonly Mock<IMediator> _mediatorMock = new Mock<IMediator>();

        [Fact]
        public async Task GetCompanyRedeems_ShouldReturnCompanyRedeemsDto_OnSuccess()
        {
            //Arrange
            IList<CompanyDto> companyRedeemDtoSet = CompanyDtoSet();
            var expectedResult = ResultModel<IList<CompanyDto>>
                .Success(200, "", companyRedeemDtoSet);

            _mediatorMock.Setup(s => s.Send(It.IsAny<GetCompaniesQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedResult);

            CompanyController sut = new CompanyController(_mediatorMock.Object);

            //Act

            await sut.GetCompanies();

            //Assert

            _mediatorMock.Setup(s =>
                s.Send(It.IsAny<GetCompaniesQuery>(), It.IsAny<CancellationToken>()));
        }

        private IList<CompanyDto> CompanyDtoSet()
        {
            return new List<CompanyDto>
            {
                new CompanyDto(1, "Burger King"),
                new CompanyDto(1, "Fitness First"),
            };
        }
    }
}