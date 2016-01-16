using System;
using System.Globalization;
using System.Windows.Data;
using lvc.Tooltip;

namespace ChartsTest.z.CustomTooltips
{
    /// <summary>
    /// Interaction logic for CustomTooltip.xaml
    /// </summary>
    public partial class CustomIndexedTooltip
    {
        public CustomIndexedTooltip()
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
