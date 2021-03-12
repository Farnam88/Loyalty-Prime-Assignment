using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using LoyaltyPrime.Services.Contexts.MemberServices.Commands;
using LoyaltyPrime.Services.Contexts.MemberServices.Dto;
using LoyaltyPrime.Services.Contexts.MemberServices.Queris;
using LoyaltyPrime.Shared.Utilities.Common.Data;
using LoyaltyPrime.WebApi.Controllers;
using MediatR;
using Moq;
using Xunit;

namespace LoyaltyPrime.ApiTests
{
    public class MemberApiTests
    {
        private readonly Mock<IMediator> _mediatorMock = new Mock<IMediator>();

        [Fact]
        public async Task CreateMember_ShouldCreatesMember_OnSuccess()
        {
            //Arrange
            var command = new CreateMemberCommand("Farnam", "Test Address");
            var expectedResult = ResultModel<int>.Success(201, "", 1);

            _mediatorMock.Setup(s => s.Send(command, It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedResult);

            MemberController sut = new MemberController(_mediatorMock.Object);

            //Act

            await sut.CreateMember(command);

            //Assert

            _mediatorMock.Setup(s => s.Send(It.IsAny<CreateMemberCommand>(), It.IsAny<CancellationToken>()));
        }

        [Fact]
        public async Task GetMembers_ShouldReturnMemberDtoSet_OnSuccess()
        {
            //Arrange
            IList<MemberDto> memberDtoSet = CreateMemberDtoSet();
            var expectedResult = ResultModel<IList<MemberDto>>.Success(200, "", memberDtoSet);

            _mediatorMock.Setup(s => s.Send(It.IsAny<GetMembersQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedResult);

            MemberController sut = new MemberController(_mediatorMock.Object);

            //Act

            await sut.GetMembers();

            //Assert

            _mediatorMock.Setup(s =>
                s.Send(It.IsAny<GetMembersQuery>(), It.IsAny<CancellationToken>()));
        }

        private IList<MemberDto> CreateMemberDtoSet()
        {
            return new List<MemberDto>
            {
                new MemberDto(1, "Farnam", "Test Address")
            };
        }
    }
}