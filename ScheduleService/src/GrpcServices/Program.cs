using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Net;
using System.Threading.Tasks;

namespace ServiceAPI
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

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
        }
            
    }
}