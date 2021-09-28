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
using TelegramBot.Models;
using TelegramBot;
using TelegramBot.Abstractions;

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
            services.ConfigureTelegramBot();
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
        private static void ConfigureTelegramBot(this IServiceCollection services)
        {
            var telegramBotSettings = _configuration.GetSection(TelegramBotSettings.Section).Get<TelegramBotSettings>();
            services.AddSingleton(telegramBotSettings);
            services.AddSingleton<ITelegramBotConnector, TelegramBotConnector>();
        }
    }
}
