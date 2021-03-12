using System.Collections.Generic;
using System.Threading.Tasks;
using LoyaltyPrime.Services.Contexts.AccountServices.Commands;
using LoyaltyPrime.Services.Contexts.AccountServices.Dto;
using LoyaltyPrime.Services.Contexts.AccountServices.Queries;
using LoyaltyPrime.WebApi.Base;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LoyaltyPrime.WebApi.Controllers
{
    [ApiController]
    [Route("api/account")]
    public class AccountController : BaseController
    {
        public AccountController(IMediator mediator) : base(mediator)
        {
        }

        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(typeof(string), 404)]
        public async Task<IActionResult> CreateAccount(CreateAccountCommand command)
        {
            var result = await Mediator.Send(command);
            return ResultStatusCode(result);
        }

        [HttpGet("{memberId}")]
        [ProducesResponseType(typeof(IList<AccountDto>), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetMemberAccounts(int memberId)
        {
            var result = await Mediator.Send(new GetMemberAccountsQuery(memberId)); 
            return ResultStatusCode(result);
        }


        [HttpGet("{memberId}/{accountId}")]
        [ProducesResponseType(typeof(AccountDto), 200)]
        [ProducesResponseType(typeof(string), 400)]
        public async Task<IActionResult> GetAccountById(int memberId, int accountId)
        {
            var result = await Mediator.Send(new GetMemberAccountQuery(memberId, accountId));
            return ResultStatusCode(result);
        }
    }
}