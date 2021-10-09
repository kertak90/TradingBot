using Models.CalculateModels;
using Models.Events;
using Models.SettingsModels;
using Models.TinkoffOpenApiModels;
using MovingAverage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TelegramBot.Abstractions;
using Tinkoff.Trading.OpenApi.Models;
using TradingCore.Abstractions;
using TradingCore.Sundries;

namespace TradingCore
{
    public class ChartIntersectionAnalyzeService : IChartIntersectionAnalyzeService
    {
        private ITradingDataService _tradingDataService { get; }
        private ChartCalculate _chartCalculate;
        private LineWeightedMovingAverage _lineWeightedMovingAverage;
        private MovingAverageSettings _movingAverageFor13DaySetting;
        private MovingAverageSettings _movingAverageFor26DaySetting;
        private ITradingEventsHandler _tradingEventsHandler;
        private readonly ITelegramBotConnector _telegramConnector;

        private HashSet<(string Ticker, string Figi)> _tickersList { get; } = new HashSet<(string Ticker, string Figi)>();

        public ChartIntersectionAnalyzeService(
            ITradingDataService tradingDataService,
            ITradingEventsHandler tradingEventsHandler,
            ITelegramBotConnector telegramConnector)
        {
            _tradingDataService = tradingDataService ?? throw new ArgumentNullException(nameof(tradingDataService));
            _tradingEventsHandler = tradingEventsHandler ?? throw new ArgumentNullException(nameof(tradingEventsHandler));
            _telegramConnector = telegramConnector ?? throw new ArgumentNullException(nameof(telegramConnector));
        }
        public async Task RunAnalyze()
        {
            await Initialize();
            await RunContinuosAnalysis();
        }
        private async Task Initialize()
        {
            var tickersList = await _tradingDataService.GetAllTickers();
            foreach (var ticker in tickersList)
                _tickersList.Add((Ticker: ticker.Ticker, Figi: ticker.Figi));

            _chartCalculate = new ChartCalculate();
            _lineWeightedMovingAverage = new LineWeightedMovingAverage();
            _movingAverageFor13DaySetting = new MovingAverageSettings()
            {
                SamplingWidth = 13,
                CenteringRule = Centering.Left
            };
            _movingAverageFor26DaySetting = new MovingAverageSettings()
            {
                SamplingWidth = 26,
                CenteringRule = Centering.Left
            };
        }
        private async Task RunContinuosAnalysis()
        {
            foreach (var ticker in _tickersList)
            {
                Console.WriteLine($"Ticker: {ticker.Ticker}");
                await AnalyzeChartsOnIntersect(ticker);
                await Task.Delay(TimeSpan.FromSeconds(1));
            }           
        }
        private async Task AnalyzeChartsOnIntersect((string Ticker, string Figi) ticker)
        {
            var tickersTimeLine = await _tradingDataService.GetTickerTimeLineForThreeMonth(ticker.Figi, CandleInterval.Day);
            if (tickersTimeLine.Count() < 60) return;
            var _candles = tickersTimeLine.Select(p =>
                            new Candle
                            {
                                figi = p.Figi,
                                interval = p.Interval.ToString(),
                                o = p.Open,
                                c = p.Close,
                                h = p.High,
                                l = p.Low,
                                v = p.Volume,
                                time = p.Time
                            });
            var movingAverageFor13Day = _lineWeightedMovingAverage
                .Calculate(_candles, _movingAverageFor13DaySetting)
                .ToArray();
            var movingAverageFor26Day = _lineWeightedMovingAverage
                .Calculate(_candles, _movingAverageFor26DaySetting)
                .ToArray();

            if (movingAverageFor13Day.Length < 2 || movingAverageFor26Day.Length < 2) return;

            var firstSection = new ChartValue[]
            {
                        movingAverageFor13Day[movingAverageFor13Day.Length - 2],
                        movingAverageFor13Day[movingAverageFor13Day.Length - 1]
            };
            var secondSection = new ChartValue[]
            {
                        movingAverageFor26Day[movingAverageFor26Day.Length - 2],
                        movingAverageFor26Day[movingAverageFor26Day.Length - 1]
            };

            if (_chartCalculate.CheckIntersectionOfSegments(firstSection, secondSection, out bool isUpwardIntersection))
            {
                if (isUpwardIntersection)
                {
                    Console.WriteLine($"${ticker.Ticker} UpWardIntersectionEvent");
                    await _telegramConnector.SendMessage($"${ticker.Ticker} UpWardIntersectionEvent");
                    _tradingEventsHandler.On(new UpWardIntersectionEvent
                    {
                        EventDayCandle = _candles.Last()
                    });
                    return;
                }
                Console.WriteLine($"${ticker.Ticker} DownWardIntersectionEvent");
                await _telegramConnector.SendMessage($"${ticker.Ticker} DownWardIntersectionEvent");
                _tradingEventsHandler.On(new DownWardIntersectionEvent
                {
                    EventDayCandle = _candles.Last()
                });
            }
        }
    }
}
