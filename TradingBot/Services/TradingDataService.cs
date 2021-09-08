using Models.TinkoffOpenApiModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Tinkoff.Trading.OpenApi.Models;
using TradingBotProjects.Services.Abstractions;

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
        public async Task GetTikerTimeLine(string figiName, DateTime timeFrom, DateTime timeTo, CandleInterval interval)
        {
            var tickerName = await _httpConnector.GetTickerName(figiName);
            var _from = timeFrom;
            var _to = timeFrom.AddDays(1);
            while (_from < timeTo)
            {
                
                var collection = await _httpConnector.GetTikerTimeLineForEveryMinutes(figiName, _from, _to, interval);
                var collectionString = "";
                foreach (var item in collection)
                {
                    collectionString += $"{item.Figi}; {item.Time}; {item.High}; {item.Interval}; {item.Low}; {item.Open}; {item.Close}; {item.Volume}\n";
                }
                await Task.Delay(200);
                _from = _from.AddDays(1);
                _to = _to.AddDays(1);
                File.AppendAllText($"{tickerName}.csv", collectionString);
            }
        }
    }
}
