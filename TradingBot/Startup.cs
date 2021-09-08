using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TradingBotProjects.Services;
using TradingBotProjects.Services.Abstractions;

namespace TradingBotProjects
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<ITradingDataService, TradingDataService>();
            services.AddScoped<IHttpConnector, TinkoffBrokerHttpConnector>();
            services.AddHttpClient<TradingDataService>(p => 
            {
                p.BaseAddress = new Uri("https://google.com");
            });

            services.AddControllers();
            services.AddMvc(options => options.EnableEndpointRouting = false);
            services.AddSwaggerGen(p =>
            {
                p.EnableAnnotations();
                p.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo()
                {
                    Version = "v1",
                    Title = "Test API"
                });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger(c =>
            {
                c.RouteTemplate = "api/swagger/{documentName}/swagger.json";
            });
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/api/swagger/v1/swagger.json", "Sample API");
                c.RoutePrefix = "api/swagger";
            });

            app.UseMvc();
        }
    }
}
