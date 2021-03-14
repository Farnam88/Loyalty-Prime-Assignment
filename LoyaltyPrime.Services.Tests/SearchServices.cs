using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LoyaltyPrime.DataAccessLayer;
using LoyaltyPrime.DataAccessLayer.Repositories;
using LoyaltyPrime.Services.Contexts.MemberServices.Dto;
using LoyaltyPrime.Services.Contexts.SearchServices.Queries;
using Moq;
using Xunit;

namespace LoyaltyPrime.Services.Tests
{
    public class SearchServices
    {
        private Mock<ISearchRepository> repositoryMock = new Mock<ISearchRepository>();

        private readonly Mock<IUnitOfWork> _unitOfWorkMock = new Mock<IUnitOfWork>();

        [Fact]
        public async Task GetMembers_ShouldReturnMemberDtoSet_OnSuccess()
        {
            //Arrange
            var memberDtoQuery = new List<MemberDto>()
                .AsQueryable();

            _unitOfWorkMock.Setup(s => s.SearchRepository).Returns(repositoryMock.Object);

            SearchQuery query = new SearchQuery();

            SearchQueryHandler sut = new SearchQueryHandler(_unitOfWorkMock.Object);

            //Act

            var result = await sut.Handle(query, It.IsAny<CancellationToken>());

            //Assert

            _unitOfWorkMock.Verify(v => v.SearchRepository);

            Assert.NotNull(result);
        }
    }
}