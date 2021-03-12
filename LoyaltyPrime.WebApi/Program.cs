using System.Threading.Tasks;
using LoyaltyPrime.WebApi.Modules;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace LoyaltyPrime.WebApi
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            await host.Services.InitDataBase();

            await host.RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
        }
    }
}