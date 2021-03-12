using System.Collections.Generic;
using System.Threading.Tasks;
using LoyaltyPrime.Services.Contexts.CompanyRewardServices.Commands;
using LoyaltyPrime.Services.Contexts.CompanyRewardServices.Dto;
using LoyaltyPrime.Services.Contexts.CompanyRewardServices.Queries;
using LoyaltyPrime.WebApi.Base;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LoyaltyPrime.WebApi.Controllers
{
    [ApiController]
    [Route("api/company-reward")]
    public class CompanyRewardController : BaseController
    {
        public CompanyRewardController(IMediator mediator) : base(mediator)
        {
        }

        [HttpPost]
        [ProducesResponseType(typeof(string), 201)]
        [ProducesResponseType(typeof(string), 404)]
        public async Task<IActionResult> CreateReward(CreateCompanyRewardCommand command)
        {
            var result = await Mediator.Send(command, default);
            return ResultStatusCode(result);
        }

        [HttpGet("{companyId}")]
        [ProducesResponseType(typeof(IList<CompanyRewardDto>), 200)]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(string),404)]
        public async Task<IActionResult> GetCompanyRewards(int companyId)
        {
            var result = await Mediator.Send(new GetCompanyRewardsQuery(companyId), default);
            return ResultStatusCode(result);
        }
    }
}