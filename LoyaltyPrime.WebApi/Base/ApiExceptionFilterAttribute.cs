using System.Collections.Generic;
using LoyaltyPrime.Shared.Utilities.Common.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace LoyaltyPrime.WebApi.Base
{
    public class ApiExceptionFilterAttribute : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            var expetion = context.Exception;
            var result = ResultModel<object>.Fail(500,
                "An unhandled error occurred",
                ErrorTypes.InternalSystemError,
                new Dictionary<string, string>
                {
                    {"Exception Error", expetion.Message}
                });
            context.Result = new ObjectResult(result)
            {
                StatusCode = 500,
            };
            context.ExceptionHandled = true;
            base.OnException(context);
        }
    }
}