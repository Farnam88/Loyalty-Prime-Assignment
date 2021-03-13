using LoyaltyPrime.DataAccessLayer.Infrastructure.Repositories;
using LoyaltyPrime.DataAccessLayer.Repositories;
using LoyaltyPrime.Models;
using Microsoft.Extensions.DependencyInjection;

namespace LoyaltyPrime.DataAccessLayer.Infrastructure.Modules
{
    public static class DataAccessLayerDiModule
    {
        public static void DataAccessLayer(this IServiceCollection services)
        {
            services.AddScoped<IRepository<Company>, Repository<Company>>();
            services.AddScoped<IRepository<Account>, Repository<Account>>();
            services.AddScoped<IRepository<Member>, Repository<Member>>();
            services.AddScoped<ISearchRepository, SearchRepository>();
            services.AddTransient<IUnitOfWork, UnitOfWork>();
        }
    }
}