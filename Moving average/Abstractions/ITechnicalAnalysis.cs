using Models.TinkoffOpenApiModels;
using System.Collections.Generic;

namespace MovingAverage.Abstractions
{
    interface ITechnicalAnalysis
    {
        IEnumerable<ChartValue> Calculate(IEnumerable<Candle> candles);
    }
}
