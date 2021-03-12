using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using LoyaltyPrime.Models.Bases.Enums;
using LoyaltyPrime.Services.Contexts.AccountServices.Commands;
using LoyaltyPrime.Services.Contexts.AccountServices.Dto;
using LoyaltyPrime.Services.Contexts.AccountServices.Queries;
using LoyaltyPrime.Shared.Utilities.Common.Data;
using LoyaltyPrime.WebApi.Controllers;
using MediatR;
using Moq;
using Xunit;

namespace LoyaltyPrime.ApiTests
{
    public class AccountApiTests
    {
        private readonly Mock<IMediator> _mediatorMock = new Mock<IMediator>();

        [Fact]
        public async Task GetMemberAccounts_ShouldReturnMemberAccounts_OnSuccess()
        {
            //Arrange
            int memberId = 1;

            var accountDtoSet = AccountDtoSet();

            var expectedResult = ResultModel<IList<AccountDto>>.Success(200, "", accountDtoSet);

            _mediatorMock.Setup(s => s.Send(It.IsAny<GetMemberAccountsQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedResult);

            AccountController sut = new AccountController(_mediatorMock.Object);

            //Act

            await sut.GetMemberAccounts(memberId);

            //Assert

            _mediatorMock.Setup(s => s.Send(It.IsAny<IRequest<IList<AccountDto>>>(), It.IsAny<CancellationToken>()));
        }

        [Fact]
        public async Task GetMemberAccountByMemberIdAndAccountId_ShouldReturnMemberAccounts_OnSuccess()
        {
            //Arrange
            int memberId = 1;
            int accountId = 1;

            var accountDto = new AccountDto(1, 1, 1,
                "Burger King", "Farnam",
                150, AccountStatus.Active.ToString());
            var expectedResult = ResultModel<AccountDto>.Success(200, "", accountDto);

            _mediatorMock.Setup(s => s.Send(It.IsAny<GetMemberAccountQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedResult);

            AccountController sut = new AccountController(_mediatorMock.Object);

            //Act

            await sut.GetAccountById(memberId, accountId);

            //Assert

            _mediatorMock.Setup(s => s.Send(It.IsAny<GetMemberAccountQuery>(), It.IsAny<CancellationToken>()));
        }

        [Fact]
        public async Task CreateAccount_ShouldCreateAccount_OnSuccess()
        {
            //Arrange
            var command = new CreateAccountCommand(1, 1);
            var expectedResult = ResultModel<int>.Success(201, "", 1);

            _mediatorMock.Setup(s => s.Send(command, It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedResult);

            AccountController sut = new AccountController(_mediatorMock.Object);

            //Act

            await sut.CreateAccount(command);

            //Assert

            _mediatorMock.Setup(s =>
                s.Send(It.IsAny<CreateAccountCommand>(), It.IsAny<CancellationToken>()));
        }


        public IList<AccountDto> AccountDtoSet()
        {
            return new List<AccountDto>
            {
                new AccountDto(1, 1, 1, "Burger King", "Farnam", 150, AccountStatus.Active.ToString()),
                new AccountDto(1, 1, 1, "Lufthansa", "Farnam", 50, AccountStatus.Inactive.ToString()),
            };
        }
    }
}