using LiveCharts;
using LiveCharts.Defaults;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace UWP.CartesianChart.Customized_Series
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CustomizedLineSeries : Page
    {
        public CustomizedLineSeries()
        {
            InitializeComponent();

            Values1 = new ChartValues<ObservableValue>
            {
                new ObservableValue(3),
                new ObservableValue(4),
                new ObservableValue(6),
                new ObservableValue(3),
                new ObservableValue(2),
                new ObservableValue(6)
            };
            Values2 = new ChartValues<ObservableValue>
            {
                new ObservableValue(5),
                new ObservableValue(3),
                new ObservableValue(5),
                new ObservableValue(7),
                new ObservableValue(3),
                new ObservableValue(9)
            };

            DataContext = this;
        }

        public ChartValues<ObservableValue> Values1 { get; set; }
        public ChartValues<ObservableValue> Values2 { get; set; }

        private void MoveOnClick(object sender, RoutedEventArgs e)
        {
            var r = new Random();

            foreach (var value in Values1)
            {
                value.Value = r.Next(0, 10);
            }
            foreach (var value in Values2)
            {
                value.Value = r.Next(0, 10);
            }
        }
    }
}
