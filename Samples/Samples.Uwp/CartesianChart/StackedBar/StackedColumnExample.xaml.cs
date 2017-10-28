using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Uwp;
using System;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace UWP.CartesianChart.StackedBar
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class StackedColumnExample : Page
    {
        public StackedColumnExample()
        {
            InitializeComponent();

            SeriesCollection = new SeriesCollection
            {
                new StackedColumnSeries
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
                    DataLabels = true
                },
                new StackedColumnSeries
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
                    DataLabels = true
                },
                new StackedColumnSeries
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
                    DataLabels = true
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
