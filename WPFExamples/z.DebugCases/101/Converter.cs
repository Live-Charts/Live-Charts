using System;
using System.Globalization;
using System.Windows.Data;
using LiveCharts;

namespace ChartsTest.z.DebugCases._101
{
    public class Converter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var v = value as double[];

            if (v == null) return null;

            return new SeriesCollection
            {
                new PieSeries {Values = new ChartValues<double> {v[0]} },
                new PieSeries {Values = new ChartValues<double> {v[1]} },
                new PieSeries {Values = new ChartValues<double> {v[2]} }
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
