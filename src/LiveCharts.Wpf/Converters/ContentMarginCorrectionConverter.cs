using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using LiveCharts.Core.Interaction.Controls;
using LiveCharts.Core.Interaction.Points;

namespace LiveCharts.Wpf.Converters
{
    public class ContentMarginCorrectionConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(values[0] is ToolTipPosition position) ||
                !(values[1] is double wedge) ||
                !(values[2] is double hyp))
            {
                return null;
            }

            double t = 0d, l = 0d, r = 0d, b = 0d;

            double alpha = wedge * Math.PI / 180d;
            double overFlow = Math.Cos(alpha / 2d) * hyp;

            switch (position)
            {
                case ToolTipPosition.Bottom:
                    t = overFlow;
                    break;
                case ToolTipPosition.Left:
                    r = overFlow;
                    break;
                case ToolTipPosition.Right:
                    l = overFlow;
                    break;
                case ToolTipPosition.Top:
                    b = overFlow;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return new Thickness(l, t, r, b);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}