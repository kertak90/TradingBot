using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;
using System.Threading;
using TradingCore.Abstractions;

namespace TradingBotProjects.Services
{
    public class ContinuousAnalysisHostedService : IHostedService
    {
        private readonly IChartIntersectionAnalyzeService _chartIntersectionAnalyzeService;

        public ContinuousAnalysisHostedService(IChartIntersectionAnalyzeService chartIntersectionAnalyzeService)
        {
            _chartIntersectionAnalyzeService = chartIntersectionAnalyzeService ?? throw new ArgumentNullException(nameof(chartIntersectionAnalyzeService));
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await _chartIntersectionAnalyzeService.RunAnalyze();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }        
    }
}
