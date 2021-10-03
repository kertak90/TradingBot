using Models;
using Models.TinkoffOpenApiModels;

namespace TradingCore.Sundries
{
    public class ChartCalculate
    {
        public bool CheckIntersectionOfSegments(ChartValue[] firstChart, ChartValue[] secondChart, out bool isUpward)
        {
            var A = GetValuePair(firstChart[0]);
            var B = GetValuePair(firstChart[1]);
            var C = GetValuePair(secondChart[0]);
            var D = GetValuePair(secondChart[1]);

            var k1 = (B.Y - A.Y) / (B.X - A.X);
            var k2 = (D.Y - C.Y) / (D.X - C.X);

            var y01 = A.Y - k1 * A.X;
            var y02 = C.Y - k2 * C.X;

            var t = (y02 - y01) / (k1 - k2);

            isUpward = true;

            if (t < A.X || t > B.X)
            {                
                return false;
            }

            if (B.Y > D.Y)
                isUpward = !isUpward;
            return true;
        }
        private ChartValuePair GetValuePair(ChartValue chartValue)
        {
            var newModel = new ChartValuePair
            {
                X = chartValue.Time.DayOfYear,
                Y = (double)chartValue.Value
            };
            return newModel;
        }
    }
}
