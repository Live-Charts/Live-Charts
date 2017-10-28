using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Wpf.CartesianChart
{
    /// <summary>
    /// Interaction logic for GroupedStackedRowExample.xaml
    /// </summary>
    public partial class GroupedStackedRowExample : UserControl
    {
        public GroupedStackedRowExample()
        {
            InitializeComponent();

            SeriesCollection = new SeriesCollection
            {
                new StackedRowSeries
                {
                    Values = new ChartValues<ObservableValue>
                    {
                        new ObservableValue(5),
                        new ObservableValue(8),
                        new ObservableValue(2),
                        new ObservableValue(4),
                        new ObservableValue(6),
                        new ObservableValue(2),
                        new ObservableValue(9),
                        new ObservableValue(3)
                    },
                    DataLabels = true,
                    Grouping = "Credits"
                },
                new StackedRowSeries
                {
                    Values = new ChartValues<ObservableValue>
                    {
                        new ObservableValue(7),
                        new ObservableValue(4),
                        new ObservableValue(1),
                        new ObservableValue(7),
                        new ObservableValue(2),
                        new ObservableValue(7),
                        new ObservableValue(0),
                        new ObservableValue(3)
                    },
                    DataLabels = true,
                    Grouping = "Credits"
                },
                new StackedRowSeries
                {
                    Values = new ChartValues<ObservableValue>
                    {
                        new ObservableValue(6),
                        new ObservableValue(2),
                        new ObservableValue(8),
                        new ObservableValue(2),
                        new ObservableValue(9),
                        new ObservableValue(2),
                        new ObservableValue(3),
                        new ObservableValue(3)
                    },
                    DataLabels = true,
                    Grouping = "Debits"
                },
                new StackedRowSeries
                {
                    Values = new ChartValues<ObservableValue>
                    {
                        new ObservableValue(2),
                        new ObservableValue(4),
                        new ObservableValue(7),
                        new ObservableValue(1),
                        new ObservableValue(9),
                        new ObservableValue(3),
                        new ObservableValue(4),
                        new ObservableValue(8)
                    },
                    DataLabels = true,
                    Grouping = "Debits"
                }
            };

            Labels = new[]
            {
                "Jan", "Feb","Mar", "Apr", "May", "Jun", "Jul", "Ago"
            };

            DataContext = this;
        }

        public SeriesCollection SeriesCollection { get; set; }
        public string[] Labels { get; set; }


        private void UpdateAllOnClick(object sender, RoutedEventArgs e)
        {
            var r = new Random();
            foreach (var series in SeriesCollection)
            {
                foreach (var observableValue in series.Values.Cast<ObservableValue>())
                {
                    observableValue.Value = r.Next(-10, 10);
                }
            }
        }
    }
}
