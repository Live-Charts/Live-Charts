using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace ChartsTest.Line_Examples.Generic
{
    public class BestSellersConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var salesData = value as MonthSalesData;
            return salesData == null
                ? ""
                : salesData.BestSellers.Aggregate((x, y) => x + ", " + y);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}