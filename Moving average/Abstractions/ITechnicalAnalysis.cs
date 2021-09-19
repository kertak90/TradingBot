using Models.SettingsModels;
using Models.TinkoffOpenApiModels;
using System.Collections.Generic;

namespace MovingAverage.Abstractions
{
    public interface ITechnicalAnalysis
    {
        IEnumerable<ChartValue> Calculate(IEnumerable<Candle> candles, MovingAverageSettings setting);
    }
}
