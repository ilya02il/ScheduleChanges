using ChatBot;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using Telegram.Bot;
using WebAPI.Configurations;
using WebAPI.Services;

namespace WebAPI
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public IConfiguration Configuration { get; set; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            string grpcChatBotServiceConnection, tgBotToken, hostAddress;

            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development")
            {
                grpcChatBotServiceConnection = Configuration.GetConnectionString("ChatBotGrpcServiceConnection");

                var tgBotConfig = Configuration.GetSection("TelegramBotConfiguration")
                    .Get<TelegramBotConfiguration>();

                tgBotToken = tgBotConfig.Token;
                hostAddress = tgBotConfig.HostAddress;
            }

            else
            {
                grpcChatBotServiceConnection = Environment.GetEnvironmentVariable("GRPC_CHAT_BOT_SRV_CONNECTION");
                tgBotToken = Environment.GetEnvironmentVariable("TG_BOT_TOKEN");
                hostAddress = Environment.GetEnvironmentVariable("HOST");
            }

            services.AddHostedService<ConfigureWebhook>();

            services.AddHttpClient("tgwebhook")
                .AddTypedClient<ITelegramBotClient>(httpClient
                    => new TelegramBotClient(tgBotToken, httpClient));

            services.AddGrpcClient<GrpcChatBot.GrpcChatBotClient>(options =>
            {
                options.Address = new Uri(grpcChatBotServiceConnection);
            });

            services.AddScoped<TelegramBotService>();

            services.AddControllers()
                .AddNewtonsoftJson();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseCors();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
