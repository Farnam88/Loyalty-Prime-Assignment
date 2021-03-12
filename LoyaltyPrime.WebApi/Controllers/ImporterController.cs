using System.Collections.Generic;
using System.Threading.Tasks;
using LoyaltyPrime.Services.Contexts.ImporterServices.Commands;
using LoyaltyPrime.Services.Contexts.ImporterServices.Models;
using LoyaltyPrime.WebApi.Base;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LoyaltyPrime.WebApi.Controllers
{
    [ApiController]
    [Route("api/importer")]
    public class ImporterController : BaseController
    {
        public ImporterController(IMediator mediator) : base(mediator)
        {
        }

        [HttpPost]
        [ProducesResponseType(200)]
        public async Task<IActionResult> ImportFromJson(List<ImportModel> input)
        {
            var result = await Mediator.Send(new ImporterCommand(input), default);
            return StatusCode(result.StatusCode);
        }
    }
}