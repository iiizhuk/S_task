using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public static class CommonHelper
    {
        public static string TwoDecimal(double value) => value.ToString("N4");
    }
}
