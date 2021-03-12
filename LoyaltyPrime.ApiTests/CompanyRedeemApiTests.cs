using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using LoyaltyPrime.Services.Contexts.CompanyRedeemServices.Commands;
using LoyaltyPrime.Services.Contexts.CompanyRedeemServices.Dto;
using LoyaltyPrime.Services.Contexts.CompanyRedeemServices.Queries;
using LoyaltyPrime.Services.Contexts.MemberServices.Queris;
using LoyaltyPrime.Shared.Utilities.Common.Data;
using LoyaltyPrime.WebApi.Controllers;
using MediatR;
using Moq;
using Xunit;

namespace LoyaltyPrime.ApiTests
{
    public class CompanyRedeemApiTests
    {
        private readonly Mock<IMediator> _mediatorMock = new Mock<IMediator>();

        [Fact]
        public async Task CreateCompanyRedeem_ShouldCreatesCompanyRedeem_OnSuccess()
        {
            //Arrange

            var command = new CreateCompanyRedeemCommand(1, "International Flight", 150);

            var expectedResult = ResultModel<int>.Success(201, "", 1);

            _mediatorMock.Setup(s => s.Send(command, It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedResult);

            CompanyRedeemController sut = new CompanyRedeemController(_mediatorMock.Object);

            //Act

            await sut.CreateRedeem(command);

            //Assert

            _mediatorMock.Setup(s => s.Send(It.IsAny<CreateCompanyRedeemCommand>(), It.IsAny<CancellationToken>()));
        }

        [Fact]
        public async Task GetCompanyRedeems_ShouldReturnCompanyRedeemsDto_OnSuccess()
        {
            //Arrange
            IList<CompanyRedeemDto> companyRedeemDtoSet = CompanyRedeemDtoSet();
            var expectedResult = ResultModel<IList<CompanyRedeemDto>>
                .Success(200, "", companyRedeemDtoSet);

            _mediatorMock.Setup(s => s.Send(It.IsAny<GetCompanyRedeemsQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedResult);

            CompanyRedeemController sut = new CompanyRedeemController(_mediatorMock.Object);

            //Act

            await sut.GetCompanyRedeems(1);

            //Assert

            _mediatorMock.Setup(s =>
                s.Send(It.IsAny<GetMembersQuery>(), It.IsAny<CancellationToken>()));
        }

        private IList<CompanyRedeemDto> CompanyRedeemDtoSet()
        {
            return new List<CompanyRedeemDto>
            {
                new CompanyRedeemDto(1, "Burger King", "Free Crispy Chicken", 100,1),
                new CompanyRedeemDto(1, "Burger King", "Free Whoopper", 120,2),
            };
        }
    }
}