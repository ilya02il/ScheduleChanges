using System.Data.Common;
using Infrastructure;
using Infrastructure.WriteData;
using JwtValidation.Messages;
using JwtValidation.Service;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using ScheduleService.ServiceAPI.GrpcClients;
using Testcontainers.MsSql;
using Xunit;

namespace Tests.Integration.Helpers
{
    public class TestWebApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
    {
        private readonly MsSqlContainer _dbContainer = new MsSqlBuilder()
            .WithPassword("93phCKFh_")
            .Build();

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureTestServices(ConfigureServices);
            builder.UseTestServer();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            var dbContextDescriptor = services
                .SingleOrDefault(descriptor => descriptor.ServiceType == typeof(DbContextOptions<EfWriteDbContext>));

            services.Remove(dbContextDescriptor);

            var dbConnectionDescriptor = services
                .SingleOrDefault(descriptor => descriptor.ServiceType == typeof(DbConnection));

            services.Remove(dbConnectionDescriptor);
            
            services.AddInfrastructure(_dbContainer.GetConnectionString());

            services
                .AddScoped(_ =>
                {
                    var callMock = GrpcCallHelpers
                        .CreateAsyncUnaryCall(new ValidateJwtTokenResponse { IsValid = true });

                    var clientMock = new Mock<GrpcJwtValidationService.GrpcJwtValidationServiceClient>();

                    clientMock.Setup(cm => cm.ValidateJwtTokenAsync(
                            It.IsAny<ValidateJwtTokenRequest>(),
                            null,
                            null,
                            It.IsAny<CancellationToken>()
                            )
                        )
                        .Returns(callMock);

                    return clientMock.Object;
                });

            services.AddScoped<JwtValidationServiceGrpcClient>();

            services
                .AddControllers()
                .AddApplicationPart(typeof(Program).Assembly);

            var provider = services.BuildServiceProvider();
            using var scope = provider.CreateScope();

            var writeContext = scope.ServiceProvider.GetRequiredService<EfWriteDbContext>();
            writeContext.Database.EnsureCreated();

            DatabaseSeeds.Seed(writeContext);
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
