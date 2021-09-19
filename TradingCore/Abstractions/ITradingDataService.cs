using Models.TinkoffOpenApiModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tinkoff.Trading.OpenApi.Models;

namespace TradingBotProjects.Services.Abstractions
{
    public interface ITradingDataService
    {
        Task<IEnumerable<MarketInstrument>> GetAllTickers();
        Task SaveTikerTimeLine(string figiName, DateTime timeFrom, DateTime timeTo, CandleInterval interval);
        Task<IEnumerable<CandlePayload>> GetTickerTimeLineForThreeMonth(string figiName, CandleInterval interval);
    }
}
