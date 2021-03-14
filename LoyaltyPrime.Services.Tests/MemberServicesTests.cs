using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using LoyaltyPrime.DataAccessLayer;
using LoyaltyPrime.DataAccessLayer.Repositories;
using LoyaltyPrime.Models;
using LoyaltyPrime.Services.Common.Specifications.MemberSpec;
using LoyaltyPrime.Services.Contexts.MemberServices.Commands;
using LoyaltyPrime.Services.Contexts.MemberServices.Dto;
using LoyaltyPrime.Services.Contexts.MemberServices.Queris;
using Moq;
using Xunit;

namespace LoyaltyPrime.Services.Tests
{
    public class MemberServicesTests
    {
        private Mock<IRepository<Member>> repositoryMock = new Mock<IRepository<Member>>();

        private readonly Mock<IUnitOfWork> _unitOfWorkMock = new Mock<IUnitOfWork>();

        [Fact]
        public async Task CreateMember_ShouldCreateMember_OnSuccess()
        {
            //Arrange
            repositoryMock.Setup(s => s.AddAsync(It.IsAny<Member>(), It.IsAny<CancellationToken>()));
            _unitOfWorkMock.Setup(s => s.MemberRepository).Returns(repositoryMock.Object);
            CreateMemberCommand command = new CreateMemberCommand("Farnam", "This is a Test Address");
            CreateMemberCommandHandler sut = new CreateMemberCommandHandler(_unitOfWorkMock.Object);
            //Act
            var result = await sut.Handle(command, It.IsAny<CancellationToken>());

            //Assert
            _unitOfWorkMock.Verify(v => v.MemberRepository);
            repositoryMock.Verify(v => v.AddAsync(It.IsAny<Member>(), It.IsAny<CancellationToken>()));
            _unitOfWorkMock.Verify(v => v.CommitAsync(It.IsAny<CancellationToken>()));
            Assert.True(result.IsSucceeded);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task GetMembers_ShouldReturnMemberDtoSet_OnSuccess(bool withResultSet)
        {
            //Arrange
            var members = MemberDtoSet();
            var emptySet = new List<MemberDto>();

            repositoryMock.Setup(s =>
                    s.GetAllAsync(It.IsAny<GetMembersDtoSpecification>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(withResultSet ? members : emptySet).Verifiable();

            _unitOfWorkMock.Setup(s => s.MemberRepository).Returns(repositoryMock.Object);

            GetMembersQuery query = new GetMembersQuery();

            GetMembersQueryHandler sut = new GetMembersQueryHandler(_unitOfWorkMock.Object);

            //Act

            var result = await sut.Handle(query, It.IsAny<CancellationToken>());

            //Assert

            _unitOfWorkMock.Verify(v => v.MemberRepository);

            repositoryMock.Verify(v =>
                v.GetAllAsync(It.IsAny<GetMembersDtoSpecification>(), It.IsAny<CancellationToken>()));

            Assert.True(result.IsSucceeded);
            if (withResultSet)
                Assert.True(result.StatusCode == 200 && result.Result != null);
            if (!withResultSet)
                Assert.True(result.StatusCode == 204 && result.Result == null);
        }

        private List<MemberDto> MemberDtoSet()
        {
            return new List<MemberDto>()
            {
                new MemberDto(1, "Farnam", "Test Address"),
                new MemberDto(2, "Jamshidian", "Test Address")
            };
        }
    }
}