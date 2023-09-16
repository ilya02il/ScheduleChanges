using Application;
using Infrastructure;
using Infrastructure.WriteData;
using JwtValidation.Messages;
using JwtValidation.Service;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using ServiceAPI;
using ServiceAPI.GrpcClients;
using ServiceAPI.Profiles;
using System.Threading;

namespace Tests.Integration.Helpers
{
    public class TestStartup : Startup
    {
        private readonly IConfiguration _configuration;

        public TestStartup(IConfiguration configuration, IWebHostEnvironment env) : base(configuration, env)
        {
            _configuration = configuration;
        }

        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddAutoMapper(typeof(MappingProfile).Assembly);
            services.AddApplication();
            services.AddInfrastructure(_configuration["TestDbConnectionString"]);

            services
                .AddScoped(_ =>
                {
                    var callMock = GrpcCallHelpers
                        .CreateAsyncUnaryCall(new ValidateJwtTokenResponse { IsValid = true });

                    var clientMock = new Mock<GrpcJwtValidationService.GrpcJwtValidationServiceClient>();

                    clientMock.Setup(cm => cm.ValidateJwtTokenAsync(
                            It.IsAny<ValidateJwtTokenRequest>(), null, null, It.IsAny<CancellationToken>()))
                        .Returns(callMock);

                    return clientMock.Object;
                });

            services.AddScoped<JwtValidationServiceGrpcClient>();

            services
                .AddControllers()
                .AddApplicationPart(typeof(Startup).Assembly);

            services.AddCors();

            var provider = services.BuildServiceProvider();
            using var scope = provider.CreateScope();

            var writeContext = scope.ServiceProvider.GetRequiredService<EfWriteDbContext>();
            writeContext.Database.EnsureCreated();

            DatabaseSeeds.Seed(writeContext);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public override void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseRouting();

            app.UseCors(x => x
                .SetIsOriginAllowed(_ => true)// allow any origin
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials());

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                //endpoints.MapGrpcService<DatedSchedulesService>();
                endpoints.MapControllers();
            });
        }
    }
}
