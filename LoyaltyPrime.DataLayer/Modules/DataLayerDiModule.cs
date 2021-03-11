using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace LoyaltyPrime.DataLayer.Modules
{
    public static class DataLayerDiModule
    {
        public static void AddDatalayer(this IServiceCollection services)
        {
            services.AddDbContext<LoyaltyPrimeContext>(opt =>
                opt.UseInMemoryDatabase("loyalty-prime-db")
                    .LogTo(Console.WriteLine)
                    .EnableSensitiveDataLogging());
        }
    }
}