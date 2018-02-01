using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using Assets.Models;

namespace Samples.Wpf
{
    public class SampleImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is Sample sample))
            {
                return null;
            }

            return new BitmapImage(new Uri($"Images/{sample.Id}.png", UriKind.Relative));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}