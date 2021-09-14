using Models.TinkoffOpenApiModels;
using MovingAverage.Abstractions;
using MovingAverage.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MovingAverage
{
    class LineWeightedMovingAverage : ITechnicalAnalysis
    {
        private readonly MovingAverageSettings _movingAverageSetting;

        public LineWeightedMovingAverage(MovingAverageSettings movingAverageSetting)
        {
            _movingAverageSetting = movingAverageSetting ?? throw new ArgumentNullException(nameof(movingAverageSetting));
        }
        public IEnumerable<ChartValue> Calculate(IEnumerable<Candle> candles)
        {
            var timeLineCandles = candles.ToArray();
            var chartValues = new List<ChartValue>();
            for (int i = 0; i < timeLineCandles.Length; i++)
            {
                var averageValue = GetAverageValue(i, ref timeLineCandles);
                var value = new ChartValue
                {
                    Value = averageValue,
                    Time = timeLineCandles[i].time
                };
                chartValues.Add(value);
            }
            return chartValues;            
        }
        private decimal GetAverageValue(int index, ref Candle[] candles)
        {
            var leftIndex = index - _movingAverageSetting.SamplingWidth;
            leftIndex = leftIndex < 0 ? 0 : leftIndex;
            var rightIndex = index;
            var takeCount = rightIndex - leftIndex;
            return CalculateWMA(leftIndex, takeCount, ref candles);
        }
        private decimal CalculateWMA(int leftIndex, int takeCount, ref Candle[] candles)
        {
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
