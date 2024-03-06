using Models;
using System;

namespace Common
{
    public static class ChartPointHelper
    {
        public static ChartPoint[] BuildByFunction(Func<double, double> function, double[] xValues)
        {
            var result = new ChartPoint[xValues.Length];

            var index = 0;
            foreach (var xValue in xValues)
            {
                result[index++] = new ChartPoint(xValue, function(xValue));
            }

            return result;
        }
    }
}