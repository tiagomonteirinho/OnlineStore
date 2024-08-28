using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using UF5423_SuperShop.Data;

namespace UF5423_SuperShop
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //CreateHostBuilder(args).Build().Run(); // Default CreateHostBuilder() line.
            var host = CreateHostBuilder(args).Build(); // Adapt to any operating system.
            RunSeeding(host); // Automatically populate table data if table is empty or create table if non-existent.
            host.Run();
        }

        private static void RunSeeding(IHost host)
        {
            // Using Factory design pattern.
            var scopeFactory = host.Services.GetService<IServiceScopeFactory>();
            using (var scope = scopeFactory.CreateScope())
            {
                var seeder = scope.ServiceProvider.GetService<SeedDb>();
                seeder.SeedAsync().Wait();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
