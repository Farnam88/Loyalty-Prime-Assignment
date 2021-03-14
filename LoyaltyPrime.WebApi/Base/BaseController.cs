using LoyaltyPrime.Shared.Utilities.Common.Data;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LoyaltyPrime.WebApi.Base
{
    [ProducesResponseType(typeof(ResultModel<object>), 500)]
    [ProducesResponseType(typeof(ResultModel<object>), 400)]
    public class BaseController : ControllerBase
    {
        protected readonly IMediator Mediator;

        public BaseController(IMediator mediator)
        {
            Mediator = mediator;
        }


        protected IActionResult ResultStatusCode<TResult>(ResultModel<TResult> resultModel)
        {
            if (resultModel.StatusCode == 404)
                return StatusCode(resultModel.StatusCode, resultModel.Message);
            if (resultModel.StatusCode == 400)
                return StatusCode(resultModel.StatusCode, resultModel.Message);
            if (resultModel.StatusCode == 200)
                return StatusCode(resultModel.StatusCode, resultModel.Result);
            if (resultModel.StatusCode == 201)
                return StatusCode(resultModel.StatusCode, resultModel.Message);
            if (resultModel.StatusCode == 204)
                return StatusCode(resultModel.StatusCode);
            return StatusCode(resultModel.StatusCode, resultModel);
        }
    }
}