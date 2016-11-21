using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using LiveCharts;

namespace Wpf.CartesianChart.Energy_Predictions
{
    public class OpacityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (Visibility) value == Visibility.Visible
                ? 1d
                : .2d;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class ReverseConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((SeriesCollection) value).Reverse();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
