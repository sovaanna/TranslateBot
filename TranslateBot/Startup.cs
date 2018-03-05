using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Telegram.Bot.Framework.Abstractions;

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
                .Build();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTelegramBot<TranslateBot>(_configuration.GetSection("TranslateBot"))
                .AddUpdateHandler<TranslateEnHandler>()
                .AddUpdateHandler<TranslateRuHandler>()
                .Configure();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                var source = new CancellationTokenSource();
                Task.Factory.StartNew(() =>
                {
                    Console.WriteLine("## Press Enter to stop bot manager...");
                    Console.ReadLine();
                    source.Cancel();
                });

                Task.Factory.StartNew(async () =>
                {
                    var botManager = app.ApplicationServices.GetRequiredService<IBotManager<TranslateBot>>();
                    while (!source.IsCancellationRequested)
                    {
                        await Task.Delay(3_000);
                        await botManager.GetAndHandleNewUpdatesAsync();
                    }

                    Console.WriteLine("## Bot manager stopped.");
                    Environment.Exit(0);
                }).ContinueWith(t =>
                {
                    if (t.IsFaulted) throw t.Exception;
                });
            }

            app.Run(async (context) =>
            {
                await context.Response.WriteAsync("Hello World!");
            });
        }
    }
}
