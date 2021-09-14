using Models.Calculate;
using Models.TinkoffOpenApiModels;
using MovingAverage.Abstractions;
using MovingAverage.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MovingAverage
{
    public class SimpleMovingAverage : ITechnicalAnalysis
    {
        private readonly MovingAverageSettings _simpleMovingAverageSetting;

        public SimpleMovingAverage(MovingAverageSettings simpleMovingAverageSetting)
        {
            _simpleMovingAverageSetting = simpleMovingAverageSetting ?? throw new ArgumentNullException(nameof(simpleMovingAverageSetting));
        }
        public IEnumerable<ChartValue> Calculate(IEnumerable<Candle> candles)
        {
            var timeLineCandles = candles.ToArray();
            var chartValues = new List<ChartValue>();
            for (int i = 0; i < timeLineCandles.Length; i++)
            {
                var averageValue = GetAverageValue(i, timeLineCandles);
                var value = new ChartValue
                {
                    Value = averageValue,
                    Time = timeLineCandles[i].time
                };
                chartValues.Add(value);
            }
            return chartValues;
        }
        private decimal GetAverageValue(int index, Candle[] candles)
        {
            decimal averageValue = 0;
            switch (_simpleMovingAverageSetting.CenteringRule)
            {
                case Centering.Left:
                    averageValue = GetLeftSideAverage(index, candles);
                    break;
                case Centering.Center:
                    averageValue = GetCenterAverage(index, candles);
                    break;
                case Centering.Right:
                    averageValue = GetRightSideAverage(index, candles);
                    break;
            }
            return averageValue;
        }
        private decimal GetLeftSideAverage(int index, Candle[] candles)
        {
            var leftIndex = index - _simpleMovingAverageSetting.SamplingWidth;
            leftIndex = leftIndex < 0 ? 0 : leftIndex;
            var rightIndex = index;
            var takeCount = rightIndex - leftIndex;
            return GetAverageValue(leftIndex, takeCount, candles);
        }
        private decimal GetCenterAverage(int index, Candle[] candles)
        {
            var leftIndex = index - _simpleMovingAverageSetting.SamplingWidth / 2;
            leftIndex = leftIndex < 0 ? 0 : leftIndex;
            var rightIndex = index + _simpleMovingAverageSetting.SamplingWidth / 2;
            rightIndex = rightIndex > candles.Length ? candles.Length : rightIndex;
            var takeCount = rightIndex - leftIndex;
            return GetAverageValue(leftIndex, takeCount, candles);
        }
        private decimal GetRightSideAverage(int index, Candle[] candles)
        {
            var leftIndex = index;
            leftIndex = leftIndex < 0 ? 0 : leftIndex;
            var rightIndex = index + _simpleMovingAverageSetting.SamplingWidth;
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
