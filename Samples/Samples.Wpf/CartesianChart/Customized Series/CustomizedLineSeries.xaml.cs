using System;
using System.Linq;
using System.Windows;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;

namespace Wpf.CartesianChart.Customized_Line_Series
{
    /// <summary>
    /// Interaction logic for CustomizedExample.xaml
    /// </summary>
    public partial class CustomizedLineSeries 
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
