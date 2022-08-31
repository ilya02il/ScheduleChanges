using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Configurations;
using DotNet.Testcontainers.Containers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Application.Tests.Integration.Helpers
{
    public class TestWebApplicationFactory<TStartup, TTestStartup> 
        : WebApplicationFactory<TStartup>, IAsyncLifetime
        where TStartup : class
        where TTestStartup : class, TStartup
    {
        private readonly MsSqlTestcontainer _dbContainer =
            new TestcontainersBuilder<MsSqlTestcontainer>()
                        .WithDatabase(new MsSqlTestcontainerConfiguration
                        {
                            Password = "93phCKFh_"
                        })
                        .Build();

        protected override IHostBuilder CreateHostBuilder()
        {
            return Host.CreateDefaultBuilder()
                .ConfigureAppConfiguration(conf =>
                {
                    conf.AddInMemoryCollection(new KeyValuePair<string, string>[]
                    {
                        new("TestDbConnectionString", _dbContainer.ConnectionString)
                    });
                });
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseStartup<TTestStartup>();
            base.ConfigureWebHost(builder);
        }

        public async Task InitializeAsync()
        {
            await _dbContainer.StartAsync();
        }

        public async Task DisposeAsync()
        {
            await _dbContainer.StopAsync();
        }
    }
}
