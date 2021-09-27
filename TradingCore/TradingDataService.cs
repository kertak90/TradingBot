using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Tinkoff.Trading.OpenApi.Models;
using TradingBotProjects.Services.Abstractions;
using TradingCore.Sundries;

namespace TradingBotProjects.Services
{
    public class TradingDataService : ITradingDataService
    {
        private IHttpConnector _httpConnector;
        public TradingDataService(IHttpConnector httpConnector)
        {
            _httpConnector = httpConnector;
        }

        public async Task<IEnumerable<MarketInstrument>> GetAllTickers()
        {
            return await _httpConnector.GetTickers();
        }
        public async Task SaveTikerTimeLine(string figiName, DateTime timeFrom, DateTime timeTo, CandleInterval interval)
        {
            var tickerName = await _httpConnector.GetTickerName(figiName);
            await foreach (var batch in GetTimeLineForEveryInterval(figiName, timeFrom, timeTo, interval))
            {
                var collectionString = "";
                foreach (var item in batch)
                {
                    collectionString += $"{item.Figi}; {item.Time}; {item.High}; {item.Interval}; {item.Low}; {item.Open}; {item.Close}; {item.Volume}\n";
                }
                File.AppendAllText($"{tickerName}.csv", collectionString);
            }
        }
        public async Task<IEnumerable<CandlePayload>> GetTickerTimeLineForThreeMonth(string figiName, CandleInterval interval)
        {
            var candlePayLoadCollection = new List<CandlePayload>();
            var from = DateTime.Now.AddMonths(-3);
            var to = DateTime.Now;
            await foreach(var batch in GetTimeLineForEveryInterval(figiName, from, to, interval))
            {
                candlePayLoadCollection.AddRange(batch);
            }
            return candlePayLoadCollection;
        }
        private async IAsyncEnumerable<IEnumerable<CandlePayload>> GetTimeLineForEveryInterval(string figiName, DateTime timeFrom, DateTime timeTo, CandleInterval interval)
        {            
            var _from = timeFrom;
            var _to = timeFrom;
            CreateTimeStep(ref _to, interval);
            while (_from < timeTo)
            {
                //todo refactor this
                if (timeTo.Subtract(_from) < TimeSpan.FromDays(1))
                    break;
                if (_to > timeTo) 
                    _to = timeTo;
                await Task.Delay(TimeSpan.FromMilliseconds(200));
                var collection = await _httpConnector.GetTikerTimeLineForEveryCandleInterval(figiName, _from, _to, interval);
                CreateTimeStep(ref _from, interval);
                CreateTimeStep(ref _to, interval);
                yield return collection;
            }
        }
        private void CreateTimeStep(ref DateTime time, CandleInterval interval)
        {
            switch(interval)
            {
                case CandleInterval.Month:
                    time = time.AddYears(1);
                    break;
                case CandleInterval.Day:
                    time = time.AddMonths(1);
                    break;
                default:
                    time = time.AddDays(1);
                    return;
            };
        }        
    }
}
