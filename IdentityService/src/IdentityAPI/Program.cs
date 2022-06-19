using IdentityAPI.Data;
using IdentityAPI.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace IdentityAPI
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            using var scope = host.Services.CreateScope();

            var services = scope.ServiceProvider;

            var context = services.GetRequiredService<ApplicationDbContext>();
            var roleManager = services.GetRequiredService<RoleManager<ApplicationRole>>();
            var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();

            context.Database.EnsureCreated();

            if (!await roleManager.RoleExistsAsync("Admin"))
            {
                var adminRole = new ApplicationRole("Admin");
                await roleManager.CreateAsync(adminRole);
            }

            if (!await roleManager.RoleExistsAsync("EducOrgManager"))
            {
                var educOrgManagerRole = new ApplicationRole("EducOrgManager");
                await roleManager.CreateAsync(educOrgManagerRole);
            }

            var adminUser = await context.Users
                .AsNoTracking()
                .SingleOrDefaultAsync(u => u.UserName == "admin");

            if (adminUser is null)
            {
                adminUser = new ApplicationUser("admin");
                await userManager.CreateAsync(adminUser, "93phCKFh_");
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }

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
                        grpcPort = 5255;
                        webApiPort = 5001;
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
                        //options.ListenAnyIP(8080, options =>
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
