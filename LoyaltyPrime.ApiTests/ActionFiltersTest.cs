using System;
using System.Collections.Generic;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using LoyaltyPrime.WebApi.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Routing;
using Xunit;

namespace LoyaltyPrime.ApiTests
{
    public class ActionFiltersTest
    {
        [Fact]
        public void ApiExceptionFilterAttribute_ShouldReturnObjectResult_WhenExceptionOccurred()
        {
            //Arrange
            var httpContext = new DefaultHttpContext();
            var context = new ExceptionContext(
                new ActionContext(httpContext, new RouteData(), new ActionDescriptor(), new ModelStateDictionary()),
                new List<IFilterMetadata>()) {Exception = new Exception("Tes Exception")};

            var sut = new ApiExceptionFilterAttribute();

            //Act
            sut.OnException(context);

            //Assert
            context.Result.Should().NotBeNull()
                .And.BeOfType<ObjectResult>();
        }

        [Fact]
        public void ApiExceptionFilterAttribute_ShouldReturnObjectResult_WhenValidationExceptionOccurred()
        {
            //Arrange
            var modelState = new ModelStateDictionary();

            modelState.AddModelError("", "error");

            var httpContext = new DefaultHttpContext();

            var context = new ExceptionContext(
                new ActionContext(httpContext, new RouteData(), new ActionDescriptor(), modelState),
                new List<IFilterMetadata>())
            {
                Exception = new ValidationException(new List<ValidationFailure>
                {
                    new ValidationFailure("MemberName", "Member is required")
                })
            };
            var sut = new ApiExceptionFilterAttribute();

            //Act

            sut.OnException(context);

            //Assert
            context.Result.Should().NotBeNull()
                .And.BeOfType<ObjectResult>();
        }
    }
}