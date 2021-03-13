using System.Collections.Generic;
using System.Threading.Tasks;
using LoyaltyPrime.Services.Contexts.MemberServices.Commands;
using LoyaltyPrime.Services.Contexts.MemberServices.Dto;
using LoyaltyPrime.Services.Contexts.MemberServices.Queris;
using LoyaltyPrime.Shared.Utilities.Common.Data;
using LoyaltyPrime.WebApi.Base;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LoyaltyPrime.WebApi.Controllers
{
    [ApiController]
    [Route("api/member")]
    public class MemberController : BaseController
    {
        public MemberController(IMediator mediator) : base(mediator)
        {
        }

        /// <summary>
        /// Get All Members
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(IList<MemberDto>), 200)]
        [ProducesResponseType(204)]
        public async Task<IActionResult> GetMembers()
        {
            var result = await Mediator.Send(new GetMembersQuery(), default);
            return ResultStatusCode(result);
        }

        [HttpPost]
        [ProducesResponseType(typeof(string), 201)]
        public async Task<IActionResult> CreateMember(CreateMemberCommand command)
        {
            var result = await Mediator.Send(command, default);
            return ResultStatusCode(result);
        }
    }
}