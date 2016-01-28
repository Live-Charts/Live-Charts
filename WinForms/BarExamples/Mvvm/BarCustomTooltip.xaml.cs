using System;
using System.Globalization;
using System.Windows.Data;

namespace WinForms.BarExamples.Mvvm
{
    /// <summary>
    /// Interaction logic for BarCustomTooltip.xaml
    /// </summary>
    public partial class BarCustomTooltip
    {
        public BarCustomTooltip()
        {
            InitializeComponent();
        }
    }

    public class BarCustomConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var sales = value as SalesInfo;
            if (sales == null) return "";
            return sales.Rentability.ToString("P");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
