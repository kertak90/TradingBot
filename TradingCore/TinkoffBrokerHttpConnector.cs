using Models.SettingsModels;
using Models.TinkoffOpenApiModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Tinkoff.Trading.OpenApi.Models;
using Tinkoff.Trading.OpenApi.Network;
using TradingCore.Abstractions;

namespace TradingBotProjects.Services
{
    public class TinkoffBrokerHttpConnector : IHttpConnector, IDisposable
    {        
        private IHttpClientFactory _httpClientFactory;
        private string _token;
        private TinkoffSettings _tinkoffSettings;
        private Connection _connection;
        private Context _context;        
        private HttpClient _httpClient;
        public TinkoffBrokerHttpConnector(IHttpClientFactory httpClientFactory,TinkoffSettings tinkoffSettings)
        {
            _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
            _tinkoffSettings = tinkoffSettings ?? throw new ArgumentNullException(nameof(tinkoffSettings));
            Initialize();
        }
        private void Initialize()
        {            
            InitializeToken();
            _connection = ConnectionFactory.GetConnection(_token);
            _context = _connection.Context;
            _httpClient = _httpClientFactory.CreateClient();
        }
        private void InitializeToken()
        {
            _token = RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
                ? File.ReadAllText(_tinkoffSettings.TokenPath.Windows)
                : File.ReadAllText(_tinkoffSettings.TokenPath.Linux);
            if (string.IsNullOrWhiteSpace(_token))
                throw new ArgumentNullException(nameof(_token));
        }
        public async Task<IEnumerable<MarketInstrument>> GetTickers()
        {
            var marketInstrumentList = await _context.MarketStocksAsync();
            return marketInstrumentList.Instruments;
        }
        public async Task<IEnumerable<CandlePayload>> GetTikerTimeLineForEveryCandleInterval(string tikerFigiName, DateTime from, DateTime to, CandleInterval interval)
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

        public void Dispose()
        {
            _httpClient.Dispose();
        }
    }
}
