using System.Reflection;
using FluentValidation;
using LoyaltyPrime.Services.Common.Behaviour;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace LoyaltyPrime.Services.Modules
{
    public static class ServicesDiModule
    {
        public static void AddApplicationServices(this IServiceCollection services)
        {
            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehaviour<,>));
        }
    }
}