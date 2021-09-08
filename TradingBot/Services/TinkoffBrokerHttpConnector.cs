using Microsoft.Extensions.Configuration;
using Models.TinkoffOpenApiModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Tinkoff.Trading.OpenApi.Models;
using Tinkoff.Trading.OpenApi.Network;
using TradingBotProjects.Services.Abstractions;

namespace TradingBotProjects.Services
{
    public class TinkoffBrokerHttpConnector : IHttpConnector
    {
        private static string Section = "TinkoffSettings";
        private IHttpClientFactory _httpClientFactory;
        private IConfiguration _configuration;
        private string _token;
        private TinkoffSettings _tinkoffSettings;

        private Connection _connection;
        private Context _context;
        
        private HttpClient _httpClient;
        public TinkoffBrokerHttpConnector(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            Initialize();
        }
        private void Initialize()
        {
            _tinkoffSettings = _configuration.GetSection(Section).Get<TinkoffSettings>();
            _token = File.ReadAllText(_tinkoffSettings.TinkoffBrokerTokenFilePath);
            _connection = ConnectionFactory.GetConnection(_token);
            _context = _connection.Context;
            _httpClient = _httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri(_tinkoffSettings.TinkoffOpenApiBaseAdress);
        }
        public async Task<IEnumerable<MarketInstrument>> GetTickers()
        {
            var marketInstrumentList = await _context.MarketStocksAsync();
            return marketInstrumentList.Instruments;
        }
        public async Task<IEnumerable<CandlePayload>> GetTikerTimeLineForEveryMinutes(string tikerFigiName, DateTime from, DateTime to, CandleInterval interval)
        {            
            var hourResults = await _context.MarketCandlesAsync(tikerFigiName, from, to, interval);
                
            return hourResults.Candles;
        }
        private async Task<IEnumerable<Candle>> GetRequestCandlePayloads(string tikerFigiName, DateTime from, DateTime to, CandleInterval interval)
        {
            var requestUrl = $"/market/candles?figi={tikerFigiName}" +
                $"&from={from.ToString()}" +
                $"&to={to.ToString()}" +
                $"&interval={interval.ToString()}";
            var responseMessage = await _httpClient.GetAsync(requestUrl);

            var content = await responseMessage.Content.ReadAsStringAsync();

            var candles = JsonConvert.DeserializeObject<CandlesResponse>(content).payload;

            return candles;
        }

        public async Task<string> GetTickerName(string figiName)
        {
            var tickerName = await _context.MarketSearchByFigiAsync(figiName);
            return tickerName.Name;
        }
    }
}
