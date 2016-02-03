using System;
using System.Globalization;
using System.Windows.Data;
using LiveCharts;
using LiveCharts.CoreComponents;

namespace ChartsTest.Pie_Examples.Mvvm
{
    /// <summary>
    /// Interaction logic for SalesTooltip.xaml
    /// </summary>
    public partial class SalesTooltip 
    {
        public SalesTooltip()
        {
            InitializeComponent();
        }
    }

    public class RentabilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var point = value as ChartPoint;
            if (point == null) return "";
            var salesData = point.Instance as SalesData;
            return salesData == null
                ? ""
                : salesData.Rentability.ToString("P");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
