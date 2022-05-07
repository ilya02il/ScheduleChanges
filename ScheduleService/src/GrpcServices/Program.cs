using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;

namespace GrpcServices
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            //using var scope = host.Services.CreateScope();

            //var services = scope.ServiceProvider;

            //try
            //{
            //var context = services.GetRequiredService<ApplicationDbContext>();

            //if (context.Database.IsSqlServer())
                //context.Database.Migrate();
            //}
            //catch (Exception exp)
            //{
            //    throw;
            //}

            await host.RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}