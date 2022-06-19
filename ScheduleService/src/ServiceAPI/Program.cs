using Infrastructure.EF;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;

namespace ServiceAPI
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            using var scope = host.Services.CreateScope();

            var services = scope.ServiceProvider;

            var context = services.GetRequiredService<ApplicationDbContext>();

            await context.Database.EnsureCreatedAsync();

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
                        grpcPort = 5200;
                        webApiPort = 5000;
                    }

                    else
                    {
                        grpcPort = Convert.ToInt32(Environment.GetEnvironmentVariable("GRPC_PORT"));
                        webApiPort = Convert.ToInt32(Environment.GetEnvironmentVariable("WEB_API_PORT"));
                    }

                    builder.UseKestrel(options =>
                    {
                        //options.ConfigureHttpsDefaults(options =>
                        //{
                        //    options.ServerCertificate =
                        //        new X509Certificate2("Certs\\cert-development.pfx", "93phCKFh_");

                        //    //options.ClientCertificateMode = ClientCertificateMode.RequireCertificate;
                        //});

                        options.ListenAnyIP(grpcPort, opt =>
                        {
                            opt.Protocols = HttpProtocols.Http2;
                        });
                        options.ListenAnyIP(webApiPort, opt =>
                        {
                            opt.Protocols = HttpProtocols.Http1;
                        });
                        //options.ListenAnyIP(80, options =>
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