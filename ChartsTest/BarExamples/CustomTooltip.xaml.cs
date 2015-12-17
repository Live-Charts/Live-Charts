using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;
using LiveCharts;
using LiveCharts.Tooltip;

namespace ChartsTest.BarExamples
{
    /// <summary>
    /// Interaction logic for CustomTooltip.xaml
    /// </summary>
    public partial class CustomTooltip
    {
        public CustomTooltip()
        {
            InitializeComponent();
        }
    }

    public class MyConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var maxSales = values[0] as decimal[][];
            var data = values[1] as IndexedTooltipData;

            if (maxSales == null || data == null) return null;

            return maxSales[data.Index][(int) data.Point.X].ToString("C");
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
