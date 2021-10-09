using Quartz;
using System;
using TradingCore.Abstractions;
using System.Threading.Tasks;

namespace TradingSchedulers.Schedulers.Jobs
{
    class ChartIntersectionAnalyzeJob : IJob
    {
        private readonly IChartIntersectionAnalyzeService _chartIntersectionAnalyzeService;

        public ChartIntersectionAnalyzeJob(IChartIntersectionAnalyzeService chartIntersectionAnalyzeService)
        {
            _chartIntersectionAnalyzeService = chartIntersectionAnalyzeService 
                ?? throw new ArgumentNullException(nameof(chartIntersectionAnalyzeService));
        }
        public async Task Execute(IJobExecutionContext context)
        {
            await _chartIntersectionAnalyzeService.RunAnalyze();
        }
    }
}
