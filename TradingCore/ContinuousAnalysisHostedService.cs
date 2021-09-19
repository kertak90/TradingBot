using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Tinkoff.Trading.OpenApi.Models;
using TradingBotProjects.Services.Abstractions;

namespace TradingBotProjects.Services
{
    public class ContinuousAnalysisHostedService : IHostedService
    {
        private ITradingDataService _tradingDataService { get; }
        private HashSet<(string Ticker, string Figi)> _tickersList { get; } = new HashSet<(string Ticker, string Figi)>();
        public ContinuousAnalysisHostedService(ITradingDataService tradingDataService)
        {
            _tradingDataService = tradingDataService ?? throw new ArgumentNullException(nameof(tradingDataService));
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await Initialize();
            await RunContinuosAnalysis(cancellationToken);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
        private async Task Initialize()
        {
            var tickersList = await _tradingDataService.GetAllTickers();
            foreach (var ticker in tickersList)
                _tickersList.Add((Ticker:ticker.Ticker, Figi:ticker.Figi));
        }
        private async Task RunContinuosAnalysis(CancellationToken cancellationToken)
        {
            while(!cancellationToken.IsCancellationRequested)
            {
                foreach(var ticker in _tickersList)
                {
                    var tickersTimeLine = await _tradingDataService.GetTickerTimeLineForThreeMonth(ticker.Figi, CandleInterval.Day);
                }
            }
        }
    }
}
