using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Models.TinkoffOpenApiModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TradingBotProjects.Services.Abstractions;

namespace TradingBotProjects.Services
{
    public static class Configure
    {
        private static IConfiguration _configuration;
        public static IServiceCollection ConfigureTradingDataService(this IServiceCollection services, IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            services.ConfigureTinkoffBrokerConnector();
            return services;
        }
        private static void ConfigureTinkoffBrokerConnector(this IServiceCollection services)
        {
            services.AddTransient<ITradingDataService, TradingDataService>();
            var tinkoffSettings = _configuration.GetSection(TinkoffSettings.Section).Get<TinkoffSettings>();
            services.AddSingleton(tinkoffSettings);
            services.AddScoped<IHttpConnector, TinkoffBrokerHttpConnector>();
            services.AddHttpClient<TradingDataService>(p =>
            {
                p.BaseAddress = new Uri(tinkoffSettings.TinkoffOpenApiBaseAdress);
            });
        }
    }
}
