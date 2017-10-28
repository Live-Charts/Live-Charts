using System;
using System.Collections.Generic;
using LiveCharts.Defaults;

namespace Wpf.CartesianChart.UIElements
{
    public static class DataProvider
    {
        public static IEnumerable<DateTimePoint> Points
        {
            get
            {
                var r = new Random();
                var timeStamp = DateTime.Now;
                var trend = 0;

                for (var i = 0; i < 50; i++)
                {
                    timeStamp = timeStamp.AddDays(1);
                    yield return new DateTimePoint(timeStamp, trend += r.Next(-20, 100));
                }
            }
        }
    }
}
