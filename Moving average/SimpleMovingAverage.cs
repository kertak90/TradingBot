using Models.CalculateModels;
using Models.SettingsModels;
using Models.TinkoffOpenApiModels;
using MovingAverage.Abstractions;
using System.Collections.Generic;
using System.Linq;

namespace MovingAverage
{
    public class SimpleMovingAverage : ITechnicalAnalysis
    {
        public IEnumerable<ChartValue> Calculate(IEnumerable<Candle> candles, MovingAverageSettings setting)
        {
            var timeLineCandles = candles.ToArray();
            var chartValues = new List<ChartValue>();
            for (int i = 0; i < timeLineCandles.Length; i++)
            {
                var averageValue = GetAverageValue(i, timeLineCandles, setting);
                var value = new ChartValue
                {
                    Value = averageValue,
                    Time = timeLineCandles[i].time
                };
                chartValues.Add(value);
            }
            return chartValues;
        }
        private decimal GetAverageValue(int index, Candle[] candles, MovingAverageSettings setting)
        {
            decimal averageValue = 0;
            switch (setting.CenteringRule)
            {
                case Centering.Left:
                    averageValue = GetLeftSideAverage(index, candles, setting);
                    break;
                case Centering.Center:
                    averageValue = GetCenterAverage(index, candles, setting);
                    break;
                case Centering.Right:
                    averageValue = GetRightSideAverage(index, candles, setting);
                    break;
            }
            return averageValue;
        }
        private decimal GetLeftSideAverage(int index, Candle[] candles, MovingAverageSettings setting)
        {
            var leftIndex = index - setting.SamplingWidth;
            leftIndex = leftIndex < 0 ? 0 : leftIndex;
            var rightIndex = index;
            var takeCount = rightIndex - leftIndex;
            return GetAverageValue(leftIndex, takeCount, candles);
        }
        private decimal GetCenterAverage(int index, Candle[] candles, MovingAverageSettings setting)
        {
            var leftIndex = index - setting.SamplingWidth / 2;
            leftIndex = leftIndex < 0 ? 0 : leftIndex;
            var rightIndex = index + setting.SamplingWidth / 2;
            rightIndex = rightIndex > candles.Length ? candles.Length : rightIndex;
            var takeCount = rightIndex - leftIndex;
            return GetAverageValue(leftIndex, takeCount, candles);
        }
        private decimal GetRightSideAverage(int index, Candle[] candles, MovingAverageSettings setting)
        {
            var leftIndex = index;
            leftIndex = leftIndex < 0 ? 0 : leftIndex;
            var rightIndex = index + setting.SamplingWidth;
            rightIndex = rightIndex > candles.Length ? candles.Length : rightIndex;
            var takeCount = rightIndex - leftIndex;
            return GetAverageValue(leftIndex, takeCount, candles);
        }
        private decimal GetAverageValue(int leftIndex, int takeCount, Candle[] candles)
        {
            return candles
                .Skip(leftIndex)
                .Take(takeCount)
                .Average(p => p.c);
        }
    }
}
