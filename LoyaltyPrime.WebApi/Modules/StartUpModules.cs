using System;
using System.Threading.Tasks;
using LoyaltyPrime.DataAccessLayer.Infrastructure.Modules;
using LoyaltyPrime.DataLayer;
using LoyaltyPrime.DataLayer.Modules;
using LoyaltyPrime.Services.Modules;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace LoyaltyPrime.WebApi.Modules
{
    public static class StartUpModules
    {
        public static void RegisterApplicationServices(this IServiceCollection services)
        {
            services.AddControllers();
            services.AddDatalayer();
            services.DataAccessLayer();
            services.AddApplicationServices();
            services.AddOData();
        }

        public static async Task InitDataBase(this IServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                using (var context = scope.ServiceProvider.GetRequiredService<LoyaltyPrimeContext>())
                {
                    await context.Database.EnsureDeletedAsync();
                    await context.Database.EnsureCreatedAsync();
                }
            }
        }
    }
}