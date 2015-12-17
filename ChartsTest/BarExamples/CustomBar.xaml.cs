using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace ChartsTest.BarExamples
{
    /// <summary>
    /// Interaction logic for CustomBar.xaml
    /// </summary>
    public partial class CustomBar
    {
        public CustomBar()
        {
            InitializeComponent();

            //this is an example to display custom data in a tooltip
            //the values in Series contains the number of items sold by Maria, John and Erick
            //but what if you need to display the higest price if the items sold by Maria, John or Erick?
            //the next array contains one array for each salesman indicationg the highest items sold each day
            //they are in the same order as we added series to our XAML, 
            //we will map this data with both indexes Series[i] and HighestItemValue[i], this is just for this example.
            //you can map data according to your needs.
            CustomTooltip.ExtraData = new[]
            {
                //Maria
                new[] {25.99m, 64.25m, 115.00m, 35m, 45m, 89m, 64.50m, 10m},
                //John
                new[] {26.99m, 65.25m, 116.00m, 36m, 46m, 90m, 65.50m, 11m},
                //Erick
                new[] {27.99m, 66.25m, 117.00m, 37m, 47m, 91m, 66.50m, 12m}
            };
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            //this is just to see animation everytime you click next
            Chart.ClearAndPlot();
        }

        public class MyConverter : IValueConverter
        {
            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                return null;
            }

            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            {
                throw new NotImplementedException();
            }
        }
    }
}
