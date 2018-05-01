using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using LiveCharts.Core.Interaction.Controls;
using LiveCharts.Core.Interaction.Points;
using LiveCharts.Wpf.Shapes;

namespace LiveCharts.Wpf.Converters
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="System.Windows.Data.IValueConverter" />
    public class GetWidthForDialog : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(values[0] is double width) ||
                !(values[1] is Thickness toolTipPadding) ||
                !(values[2] is DialogShape shape))
            {
                return new Size(0, 0);
            }

            var overFlow = 0d;

            if (shape.Position == ToolTipPosition.Left || shape.Position == ToolTipPosition.Right)
            {
                var alpha = shape.Wedge * Math.PI / 180d;
                overFlow = Math.Cos(alpha / 2d) * shape.WedgeHypotenuse;
            }

            return width + toolTipPadding.Left + toolTipPadding.Right + overFlow;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}