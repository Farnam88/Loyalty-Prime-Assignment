using System.Collections.Generic;
using System.Linq;
using LoyaltyPrime.Services.Contexts.Search1Services.Dto;
using LoyaltyPrime.Services.Contexts.Search1Services.Queries;
using LoyaltyPrime.WebApi.Base;
using MediatR;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Mvc;

namespace LoyaltyPrime.WebApi.Controllers
{
    [ApiController]
    [Route("api/Search")]
    [ProducesResponseType(200)]
    public class SearchController : BaseController
    {
        public SearchController(IMediator mediator) : base(mediator)
        {
        }

        [HttpGet]
        [EnableQuery]
        [ProducesResponseType(typeof(List<MemberSearchDro>), 200)]
        public IQueryable<MemberSearchDro> Search()
        {
            var result = Mediator.Send(new SearchQuery(), default)
                .ConfigureAwait(false)
                .GetAwaiter()
                .GetResult();
            return result;
        }
    }
}