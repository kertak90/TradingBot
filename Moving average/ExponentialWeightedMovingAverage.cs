using Models.TinkoffOpenApiModels;
using MovingAverage.Abstractions;
using MovingAverage.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MovingAverage
{
    class ExponentialWeightedMovingAverage : ITechnicalAnalysis
    {
        private readonly MovingAverageSettings _movingAverageSetting;

        public ExponentialWeightedMovingAverage(MovingAverageSettings movingAverageSetting)
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
            return CalculateEWMA(leftIndex, takeCount, ref candles);
        }
        private decimal CalculateEWMA(int leftIndex, int takeCount, ref Candle[] candles)
        {
            decimal summ = 0;


            return summ;
        }
    }
}
