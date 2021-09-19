using Models.SettingsModels;
using Models.TinkoffOpenApiModels;
using MovingAverage.Abstractions;
using System.Collections.Generic;
using System.Linq;

namespace MovingAverage
{
    public class ExponentialWeightedMovingAverage : ITechnicalAnalysis
    {
        public IEnumerable<ChartValue> Calculate(IEnumerable<Candle> candles, MovingAverageSettings setting)
        {
            var timeLineCandles = candles.ToArray();
            var chartValues = new List<ChartValue>();
            for (int i = 0; i < timeLineCandles.Length; i++)
            {
                var averageValue = GetAverageValue(i, ref timeLineCandles, setting);
                var value = new ChartValue
                {
                    Value = averageValue,
                    Time = timeLineCandles[i].time
                };
                chartValues.Add(value);
            }
            return chartValues;
        }
        private decimal GetAverageValue(int index, ref Candle[] candles, MovingAverageSettings setting)
        {
            var leftIndex = index - setting.SamplingWidth;
            leftIndex = leftIndex < 0 ? 0 : leftIndex;
            var rightIndex = index;
            var takeCount = rightIndex - leftIndex;
            return CalculateEWMA(leftIndex, takeCount, ref candles);
        }
        private decimal CalculateEWMA(int leftIndex, int takeCount, ref Candle[] candles)
        {
            if (takeCount <= 1) return 0;
            decimal summ = 0;
            int countOfValues = leftIndex + takeCount;
            for (int i = leftIndex; i < countOfValues; i++)
            {
                summ += candles[i].o * (2 / (countOfValues - i) + 1);
            }
            return summ / countOfValues;
        }
    }
}
