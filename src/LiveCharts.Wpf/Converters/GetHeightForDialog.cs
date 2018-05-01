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
    public class GetHeightForDialog : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(values[0] is double height) ||
                !(values[1] is Thickness toolTipPadding) ||
                !(values[2] is DialogShape shape))
            {
                return new Size(0, 0);
            }

            var overFlow = 0d;

            if (shape.Position == ToolTipPosition.Top || shape.Position == ToolTipPosition.Bottom)
            {
                var alpha = shape.Wedge * Math.PI / 180d;
                overFlow = Math.Cos(alpha / 2d) * shape.WedgeHypotenuse;
            }

            return height + toolTipPadding.Top + toolTipPadding.Bottom + overFlow;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}