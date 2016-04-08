using System;
using System.Globalization;
using System.Windows.Data;
using LiveCharts;

namespace ChartsTest.z.DebugCases._101
{
    public class Converter : IValueConverter
    {
        internal static int tester;
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var v = value as double[];

            if (v == null) return null;

            return new SeriesCollection
            {
                new PieSeries {Values = new ChartValues<double> {v[0]}, Title = tester++.ToString()},
                new PieSeries {Values = new ChartValues<double> {v[1]}, Title = tester++.ToString()},
                new PieSeries {Values = new ChartValues<double> {v[2]}, Title = tester++.ToString()}
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
