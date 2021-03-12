using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using LoyaltyPrime.Services.Contexts.CompanyRewardServices.Commands;
using LoyaltyPrime.Services.Contexts.CompanyRewardServices.Dto;
using LoyaltyPrime.Services.Contexts.CompanyRewardServices.Queries;
using LoyaltyPrime.Services.Contexts.MemberServices.Queris;
using LoyaltyPrime.Shared.Utilities.Common.Data;
using LoyaltyPrime.WebApi.Controllers;
using MediatR;
using Moq;
using Xunit;

namespace LoyaltyPrime.ApiTests
{
    public class CompanyRewardApiTests
    {
        private readonly Mock<IMediator> _mediatorMock = new Mock<IMediator>();

        [Fact]
        public async Task CreateCompanyReward_ShouldCreatesCompanyReward_OnSuccess()
        {
            //Arrange

            var command = new CreateCompanyRewardCommand(1, "International Flight", 150);

            var expectedResult = ResultModel<int>.Success(201, "", 1);

            _mediatorMock.Setup(s => s.Send(command, It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedResult);

            CompanyRewardController sut = new CompanyRewardController(_mediatorMock.Object);

            //Act

            await sut.CreateReward(command);

            //Assert

            _mediatorMock.Setup(s => s.Send(It.IsAny<CreateCompanyRewardCommand>(), It.IsAny<CancellationToken>()));
        }

        [Fact]
        public async Task GetCompanyRewards_ShouldReturnCompanyRewardsDto_OnSuccess()
        {
            //Arrange
            IList<CompanyRewardDto> companyRewardDtoSet = CompanyRewardDtoSet();
            var expectedResult = ResultModel<IList<CompanyRewardDto>>
                .Success(200, "", companyRewardDtoSet);

            _mediatorMock.Setup(s => s.Send(It.IsAny<GetCompanyRewardsQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedResult);

            CompanyRewardController sut = new CompanyRewardController(_mediatorMock.Object);

            //Act

            await sut.GetCompanyRewards(1);

            //Assert

            _mediatorMock.Setup(s =>
                s.Send(It.IsAny<GetMembersQuery>(), It.IsAny<CancellationToken>()));
        }

        private IList<CompanyRewardDto> CompanyRewardDtoSet()
        {
            return new List<CompanyRewardDto>
            {
                new CompanyRewardDto(1, "Burger King", "Crispy Chicken", 40,1),
                new CompanyRewardDto(1, "Burger King", "Whoopper", 50,2),
            };
        }
    }
}