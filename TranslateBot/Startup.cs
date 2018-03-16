using Autofac;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Telegram.Bot.Framework;
using Telegram.Bot.Framework.Abstractions;
using TranslateBot.Commands;
using TranslateService;

namespace TranslateBot
{
    public class Startup
    {
        private readonly IConfigurationRoot _configuration;

        public Startup(IHostingEnvironment env)
        {
            _configuration = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", true)
                .AddEnvironmentVariables()
                .Build();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTelegramBot<TranslateBot>(_configuration.GetSection("TranslateBot"))
                .AddUpdateHandler<EnglishTextUpdateHandler>()
                .AddUpdateHandler<RussianTextUpdateHandler>()
                .Configure();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILogger logger)
        {
            app.UseDeveloperExceptionPage();

            logger.Information("Setting webhook for {BotName}...", nameof(TranslateBot));
            app.ApplicationServices.GetRequiredService<IBotManager<TranslateBot>>();
            app.UseTelegramBotWebhook<TranslateBot>();
            logger.Information("Webhook is set for {BotName}...", nameof(TranslateBot));

            app.Run(async context => { await context.Response.WriteAsync("Hello World!"); });
        }
            
        public void ConfigureContainer(ContainerBuilder builder)
        {
            var logger = new LoggerConfiguration().WriteTo.Console().CreateLogger();
            builder.RegisterInstance(logger).AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<YandexTranslate>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterInstance(_configuration).AsImplementedInterfaces().SingleInstance();
        }
    }
}
