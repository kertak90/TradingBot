using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TradingCore;
using TradingCore.Abstractions;
using TradingSchedulers.Schedulers.Jobs;

namespace TradingSchedulers.Schedulers
{
    public static class AddChartsIntersectionAnalyzeScheduler
    {
        public static void InitializeChartIntersectionScheduler(this IServiceCollection services)
        {
            services.AddTransient<IJobFactory, IntersectionJobFactory>();
            services.AddSingleton<IJob, ChartIntersectionAnalyzeJob>();
            services.AddSingleton<IChartIntersectionAnalyzeService, ChartIntersectionAnalyzeService>();  
            services.AddHostedService<SchedulersHostedService>();
        }
    }
}
