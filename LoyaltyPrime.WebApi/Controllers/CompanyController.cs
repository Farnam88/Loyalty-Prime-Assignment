using System.Collections.Generic;
using System.Threading.Tasks;
using LoyaltyPrime.Services.Contexts.CompanyServices.Dtos;
using LoyaltyPrime.Services.Contexts.CompanyServices.Queries;
using LoyaltyPrime.Shared.Utilities.Common.Data;
using LoyaltyPrime.WebApi.Base;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LoyaltyPrime.WebApi.Controllers
{
    [ApiController]
    [Route("api/company")]
    public class CompanyController : BaseController
    {
        public CompanyController(IMediator mediator) : base(mediator)
        {
        }

        [HttpGet]
        [ProducesResponseType(typeof(IList<CompanyDto>), 200)]
        [ProducesResponseType(204)]
        public async Task<IActionResult> GetCompanies()
        {
            var result = await Mediator.Send(new GetCompaniesQuery(), default);
            return ResultStatusCode(result);
        }
    }
}