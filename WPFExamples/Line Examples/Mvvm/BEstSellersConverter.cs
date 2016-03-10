using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using LiveCharts.CoreComponents;

namespace ChartsTest.Line_Examples.Mvvm
{
    public class BestSellersConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var point = value as ChartPoint;
            if (point == null) return "";
            var salesData = point.Instance as MonthSalesData;
            return salesData == null
                ? ""
                : salesData.BestSellers.Aggregate((x, y) => x + y);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}