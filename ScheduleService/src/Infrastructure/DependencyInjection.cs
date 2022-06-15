using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Application.Common.Interfaces;
using Infrastructure.EF;
using System.Reflection;
using Infrastructure.Files;
using Domain.Entities;
using Domain.ValueObjects;
using System;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(builder =>
                builder.UseSqlServer(
                    //Environment.GetEnvironmentVariable("SQL_SERVER_CONNECTION"),
                    configuration.GetConnectionString("DefaultConnection"),
                    b => b.MigrationsAssembly(Assembly.GetExecutingAssembly().FullName)),
                ServiceLifetime.Scoped);

            services.AddScoped<IApplicationDbContext>(provider => provider.GetService<ApplicationDbContext>());
            services.AddTransient<ITableFileParser<ItemInfo>, WordTableFileParser<ItemInfo>>();

            return services;
        }
    }
}
