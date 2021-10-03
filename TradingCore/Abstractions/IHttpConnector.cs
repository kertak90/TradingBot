using Models.TinkoffOpenApiModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tinkoff.Trading.OpenApi.Models;

namespace TradingCore.Abstractions
{
    public interface IHttpConnector
    {
        Task<IEnumerable<MarketInstrument>> GetTickers();
        Task<IEnumerable<CandlePayload>> GetTikerTimeLineForEveryCandleInterval(string tikerFigiName, DateTime from, DateTime to, CandleInterval interval);
        Task<string> GetTickerName(string figiName);
    }
}
