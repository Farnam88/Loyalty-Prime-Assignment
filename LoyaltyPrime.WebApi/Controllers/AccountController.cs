using System.Collections.Generic;
using System.Threading.Tasks;
using LoyaltyPrime.Services.Contexts.AccountServices.Commands;
using LoyaltyPrime.Services.Contexts.AccountServices.Dto;
using LoyaltyPrime.Services.Contexts.AccountServices.Queries;
using LoyaltyPrime.Shared.Utilities.Common.Data;
using LoyaltyPrime.WebApi.Base;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LoyaltyPrime.WebApi.Controllers
{
    [ApiController]
    [Route("api/account")]
    [ProducesResponseType(typeof(ResultModel<object>), 500)]
    public class AccountController : BaseController
    {
        public AccountController(IMediator mediator) : base(mediator)
        {
        }

        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(typeof(string), 404)]
        public async Task<IActionResult> CreateAccount([FromBody] CreateAccountCommand command)
        {
            var result = await Mediator.Send(command);
            return ResultStatusCode(result);
        }

        [HttpGet("{memberId}")]
        [ProducesResponseType(typeof(IList<AccountDto>), 200)]
        [ProducesResponseType(204)]
        public async Task<IActionResult> GetMemberAccounts([FromRoute] int memberId)
        {
            var result = await Mediator.Send(new GetMemberAccountsQuery(memberId));
            return ResultStatusCode(result);
        }


        [HttpGet("{memberId}/{accountId}")]
        [ProducesResponseType(typeof(AccountDto), 200)]
        public async Task<IActionResult> GetAccountById([FromRoute] int memberId, [FromRoute] int accountId)
        {
            var result = await Mediator.Send(new GetMemberAccountQuery(memberId, accountId));
            return ResultStatusCode(result);
        }
    }
}