using Infrastructure.EF;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;

namespace GrpcAPI
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            using var scope = host.Services.CreateScope();

            var services = scope.ServiceProvider;

            var context = services.GetRequiredService<ApplicationDbContext>();

            if (context.Database.IsSqlServer())
                context.Database.Migrate();

            await host.RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(builder =>
                {
                    int grpcPort;
                    int webApiPort;

                    var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

                    if (env == "Development")
                    {
                        grpcPort = 5155;
                        webApiPort = 5100;
                    }

                    else
                    {
                        grpcPort = 666;
                        webApiPort = 80;
                    }

                    builder.UseKestrel(options =>
                    {
                        options.ListenAnyIP(grpcPort, opt =>
                        {
                            opt.Protocols = HttpProtocols.Http2;
                        });
                        options.ListenAnyIP(webApiPort, opt =>
                        {
                            opt.Protocols = HttpProtocols.Http1;
                        });
                        //options.ListenAnyIP(8008, options =>
                        //{
                        //    options.Protocols = HttpProtocols.Http1AndHttp2;
                        //    options.UseHttps();
                        //});
                    });

                    builder.UseStartup<Startup>();
                });
        }
    }
}