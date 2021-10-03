using Quartz;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TradingCore.Abstractions;

namespace TradingSchedulers.Schedulers.Jobs
{
    class SimpleJob : IJob
    {
        private readonly IChartIntersectionAnalyzeService chartIntersectionAnalyzeService;

        public SimpleJob(IChartIntersectionAnalyzeService chartIntersectionAnalyzeService)
        {
            this.chartIntersectionAnalyzeService = chartIntersectionAnalyzeService;
        }
        public Task Execute(IJobExecutionContext context)
        {
            Console.WriteLine("Run job");
            return Task.CompletedTask;
        }
    }
}
