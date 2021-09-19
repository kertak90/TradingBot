using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using Tinkoff.Trading.OpenApi.Models;

namespace TradingBotProjectBehaviors
{
    public class TickerControllerBehaviors : ApiIntegrationTestBase
    {
        private IEnumerable<MarketInstrument> _expectedMarketInstruments;
        private MarketInstrument _expectedMarketInstrument;
        [SetUp]
        public void Setup()
        {
            _expectedMarketInstrument = new MarketInstrument(
                "figi",
                "Spce",
                "123",
                100,
                2,
                Currency.Usd,
                "VirginGalactic",
                Tinkoff.Trading.OpenApi.Models.InstrumentType.Stock);
            
            _expectedMarketInstruments = new MarketInstrument[]
            {
                _expectedMarketInstrument
            };
            _connector.GetTickers()
                .Returns(Task.FromResult(_expectedMarketInstruments));
        }

        [Test]
        public async Task ShouldGetAllTickersWhenInvokeGetAllTickersMethod()
        {
            // Given
            var uri = "Ticker/GetAllTickers";
            
            // When
            var response = await _client.GetAsync(uri);
            
            // Then
            response.StatusCode.Should().Equals(HttpStatusCode.OK);
        }
    }
}