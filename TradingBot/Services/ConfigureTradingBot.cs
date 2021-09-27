using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TradingBotProjects.Services.Abstractions;
using MovingAverage.Abstractions;
using MovingAverage;
using Models.SettingsModels;
using TradingCore; 

namespace TradingBotProjects.Services
{
    public static class ConfigureTradingBot
    {
        private static IConfiguration _configuration;
        public static void ConfigureTradingDataService(this IServiceCollection services, IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            services.AddHostedServices();
            services.ConfigureTinkoffBrokerConnector();
            services.ConfigureTechnicalAnalysis();
            services.ConfigureEventHandler();
        }
        private static void AddHostedServices(this IServiceCollection services)
        {
            services.AddHostedService<ContinuousAnalysisHostedService>();
        }
        private static void ConfigureTinkoffBrokerConnector(this IServiceCollection services)
        {
            var tinkoffSettings = _configuration.GetSection(TinkoffSettings.Section).Get<TinkoffSettings>();
            services.AddSingleton(tinkoffSettings);
            services.AddHttpClient<TradingDataService>(p =>
            {
                p.BaseAddress = new Uri(tinkoffSettings.TinkoffOpenApiBaseAdress);
            });
            services.AddTransient<ITradingDataService, TradingDataService>();
            services.AddTransient<IHttpConnector, TinkoffBrokerHttpConnector>();
        }
        private static void ConfigureTechnicalAnalysis(this IServiceCollection services)
        {
            services.AddTransient<ITechnicalAnalysis, LineWeightedMovingAverage>();
        }
        private static void ConfigureEventHandler(this IServiceCollection services)
        {
            services.AddSingleton<ITradingEventsHandler, TradingEventsHandler>();
        }
    }
}
