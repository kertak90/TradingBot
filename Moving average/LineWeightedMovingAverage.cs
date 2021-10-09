using Models.SettingsModels;
using Models.TinkoffOpenApiModels;
using MovingAverage.Abstractions;
using System.Collections.Generic;
using System.Linq;

namespace MovingAverage
{
    public class LineWeightedMovingAverage : ITechnicalAnalysis
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
            return CalculateWMA(leftIndex, takeCount, ref candles);
        }
        private decimal CalculateWMA(int leftIndex, int takeCount, ref Candle[] candles)
        {
            if (takeCount <= 1) return 0;
            decimal summ = 0;
            int countOfValues = leftIndex + takeCount;
            for(int i = leftIndex; i < countOfValues; i++)
            {
                summ += candles[i].o * (countOfValues - i);
            }
            return summ * 2 / (takeCount * (takeCount - 1));
        }
    }
}
