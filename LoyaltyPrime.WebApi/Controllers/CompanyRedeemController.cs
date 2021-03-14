using System.Collections.Generic;
using System.Threading.Tasks;
using LoyaltyPrime.Services.Contexts.CompanyRedeemServices.Commands;
using LoyaltyPrime.Services.Contexts.CompanyRedeemServices.Dto;
using LoyaltyPrime.Services.Contexts.CompanyRedeemServices.Queries;
using LoyaltyPrime.WebApi.Base;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LoyaltyPrime.WebApi.Controllers
{
    [ApiController]
    [Route("api/company-redeem")]
    public class CompanyRedeemController : BaseController
    {
        public CompanyRedeemController(IMediator mediator) : base(mediator)
        {
        }

        [HttpPost]
        [ProducesResponseType(typeof(string), 201)]
        [ProducesResponseType(typeof(string), 404)]
        public async Task<IActionResult> CreateRedeem([FromBody]CreateCompanyRedeemCommand command)
        {
            var result = await Mediator.Send(command, default);
            return ResultStatusCode(result);
        }

        [HttpGet("{companyId}")]
        [ProducesResponseType(typeof(IList<CompanyRedeemDto>), 200)]
        [ProducesResponseType(204)]
        public async Task<IActionResult> GetCompanyRedeems([FromRoute] int companyId)
        {
            var result = await Mediator.Send(new GetCompanyRedeemsQuery(companyId), default);
            return ResultStatusCode(result);
        }
    }
}