using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace UWP.CartesianChart.Energy_Predictions
{
    public class OpacityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return (Visibility)value == Visibility.Visible
                ? 1d
                : .2d;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
