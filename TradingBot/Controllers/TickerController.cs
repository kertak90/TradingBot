using System;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Tinkoff.Trading.OpenApi.Models;
using TradingBotProjects.Services.Abstractions;
using Models.TinkoffOpenApiModels;

namespace TradingBotProjects.Controllers
{    
    [Route("api/[controller]")]
    [ApiController]
    public class TickerController : Controller
    {
        private ITradingDataService _tradingDataService;
        public TickerController(ITradingDataService tradingDataService)
        {
            _tradingDataService = tradingDataService;
        }
        [HttpGet("[action]")]
        [SwaggerOperation(Description = "GetAllTickers")]
        public async Task<IEnumerable<MarketInstrument>> GetAllTickers()
        {
            return await _tradingDataService.GetAllTickers();
        }

        [HttpPost("[action]")]
        [SwaggerOperation(Description = "time format: 2020-11-16T04:25:03")]
        public async Task SaveTickerPriceTimeLine([FromQuery] string tikerName,[FromQuery] CustomCandleInterval customInterval, [FromQuery] DateTime timeFrom, [FromQuery] DateTime timeTo)
        {
            Console.WriteLine();
            var interval = (CandleInterval)((int)customInterval);
            await _tradingDataService.GetTikerTimeLine(tikerName, timeFrom, timeTo, interval);
        }
    }
}