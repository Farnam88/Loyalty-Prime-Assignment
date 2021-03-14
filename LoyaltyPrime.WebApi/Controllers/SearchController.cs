using System.Collections.Generic;
using System.Linq;
using LoyaltyPrime.Services.Contexts.SearchServices.Dto;
using LoyaltyPrime.Services.Contexts.SearchServices.Queries;
using LoyaltyPrime.WebApi.Base;
using MediatR;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Mvc;

namespace LoyaltyPrime.WebApi.Controllers
{
    [ApiController]
    [Route("api/search")]
    public class SearchController : BaseController
    {
        public SearchController(IMediator mediator) : base(mediator)
        {
        }

        [HttpGet]
        [EnableQuery]
        [ProducesResponseType(typeof(List<MemberSearchDto>), 200)]
        public IQueryable<MemberSearchDto> Search()
        {
            var result = Mediator.Send(new SearchQuery(), default)
                .ConfigureAwait(false)
                .GetAwaiter()
                .GetResult();
            return result;
        }
    }
}