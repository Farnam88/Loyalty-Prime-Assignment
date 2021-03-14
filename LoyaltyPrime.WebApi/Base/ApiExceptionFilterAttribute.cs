using System.Collections.Generic;
using System.Linq;
using FluentValidation;
using LoyaltyPrime.Shared.Utilities.Common.Data;
using LoyaltyPrime.Shared.Utilities.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace LoyaltyPrime.WebApi.Base
{
    public class ApiExceptionFilterAttribute : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            if (context.Exception is ValidationException)
            {
                ValidationException(context);
                base.OnException(context);
            }
            else
            {
                var result = ResultModel<object>.Fail(500,
                    "An unhandled error occurred",
                    ErrorTypes.InternalSystemError,
                    new Dictionary<string, string>
                    {
                        {"Exception Error", context.Exception.Message}
                    });
                context.Result = new ObjectResult(result)
                {
                    StatusCode = 500,
                };
                context.ExceptionHandled = true;
            }
        }

        private void ValidationException(ExceptionContext context)
        {
            var exception = (ValidationException) context.Exception;
            var errors = exception.Errors
                .Where(w => w.PropertyName.ContainsString())
                .Select(s => new KeyValuePair<string, string>(s.PropertyName, s.ErrorMessage))
                .AsEnumerable();
            var result = ResultModel<object>.Fail(400,
                "Requested Data is not Valid",
                ErrorTypes.ValidationError,
                new Dictionary<string, string>(errors)
                {
                    {"Exception Error", exception.Message}
                });
            context.Result = new ObjectResult(result)
            {
                StatusCode = 400
            };
            context.ExceptionHandled = true;
        }
    }
}