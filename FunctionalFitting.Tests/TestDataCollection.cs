using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionalFitting.Tests
{
    public class TestDataCollection
    {
        //Expression<Func<double, double> probably can be used to compare result
        public static IEnumerable<(double[], double[], string)> Linear
        {
            get
            {
                yield return (
                    new double[] { 1.0, 2.0 },
                    new double[] { 13.0, 18.0 },
                    "f(x) = (5.0000 * x) + 8.0000"
                );

                yield return (
                    new double[] { 2.0, 4.0 },
                    new double[] { 2, 3 },
                    "f(x) = (0.5000 * x) + 1.0000"
                );
            }
        }
    }
}
