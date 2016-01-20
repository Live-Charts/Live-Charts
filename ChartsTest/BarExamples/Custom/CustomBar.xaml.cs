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
