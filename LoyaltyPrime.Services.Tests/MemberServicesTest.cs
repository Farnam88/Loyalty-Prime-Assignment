using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using LoyaltyPrime.DataAccessLayer;
using LoyaltyPrime.DataAccessLayer.Repositories;
using LoyaltyPrime.DataAccessLayer.Specifications;
using LoyaltyPrime.Models;
using LoyaltyPrime.Services.Contexts.MemberServices.Commands;
using LoyaltyPrime.Services.Contexts.MemberServices.Dto;
using LoyaltyPrime.Services.Contexts.MemberServices.Queris;
using Moq;
using Xunit;

namespace LoyaltyPrime.Services.Tests
{
    public class MemberServicesTest
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

        [Fact]
        public async Task GetMembers_ShouldReturnMemberDtoSet_OnSuccess()
        {
            //Arrange
            var members = MemberDtoSet();

            repositoryMock.Setup(s =>
                    s.GetAllAsync(It.IsAny<ISpecification<Member, MemberDto>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(members).Verifiable();

            _unitOfWorkMock.Setup(s => s.MemberRepository).Returns(repositoryMock.Object);

            GetMembersQuery command = new GetMembersQuery();

            GetMembersQueryHandler sut = new GetMembersQueryHandler(_unitOfWorkMock.Object);

            //Act

            var result = await sut.Handle(command, It.IsAny<CancellationToken>());

            //Assert

            _unitOfWorkMock.Verify(v => v.MemberRepository);

            repositoryMock.Verify(v =>
                v.GetAllAsync(It.IsAny<ISpecification<Member, MemberDto>>(), It.IsAny<CancellationToken>()));

            Assert.True(result.IsSucceeded);

            Assert.NotNull(result.Result);
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