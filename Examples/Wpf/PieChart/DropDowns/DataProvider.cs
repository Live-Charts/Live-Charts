using System;
using System.Collections.Generic;

namespace Wpf.PieChart.DropDowns
{
    public static class DataProvider
    {
        public static IEnumerable<double> Values
        {
            get
            {
                var r = new Random();

                for (var i = 0; i < 30; i++)
                {
                    yield return r.NextDouble() * 100;
                }
            }
        }
    }
}
