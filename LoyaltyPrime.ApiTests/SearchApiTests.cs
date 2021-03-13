using System.Linq;
using System.Threading;
using LoyaltyPrime.Services.Contexts.Search1Services.Dto;
using LoyaltyPrime.Services.Contexts.Search1Services.Queries;
using LoyaltyPrime.WebApi.Controllers;
using MediatR;
using Moq;
using Xunit;

namespace LoyaltyPrime.ApiTests
{
    public class SearchApiTests
    {
        private readonly Mock<IMediator> _mediatorMock = new Mock<IMediator>();

        [Fact]
        public void ImportFromJson_ShouldCreateMemberAndAccountAndCompany_OnSuccess()
        {
            //Arrange

            _mediatorMock.Setup(s =>
                    s.Send(It.IsAny<SearchQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(It.IsAny<IQueryable<MemberSearchDro>>())
                .Verifiable();

            SearchController sut = new SearchController(_mediatorMock.Object);

            //Act
            sut.Search();

            //Assert

            _mediatorMock.Verify(s =>
                s.Send(It.IsAny<SearchQuery>(), It.IsAny<CancellationToken>()));
        }
    }
}