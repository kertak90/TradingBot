using System;
using System.Linq;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using TradingBotProjects;
using TradingBotProjects.Services.Abstractions;

namespace TradingBotProjectBehaviors
{
    public class ApiIntegrationTestBase : IDisposable
    {
        protected WebApplicationFactory<Startup> _webApplicationFactory;
        protected IHttpConnector _connector;
        protected HttpClient _client;

        public ApiIntegrationTestBase()
        {
            _connector = Substitute.For<IHttpConnector>();
            _webApplicationFactory = new WebApplicationFactory<Startup>()
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureServices(services =>
                    {
                        var httpConnector = services
                            .SingleOrDefault(p => p.ServiceType == typeof(IHttpConnector));
                        services.Remove(httpConnector);
                        services.AddSingleton(_connector);
                    });
                });
            _client = _webApplicationFactory.CreateClient();
        }

        public void Dispose()
        {
            _webApplicationFactory?.Dispose();
        }
    }
}