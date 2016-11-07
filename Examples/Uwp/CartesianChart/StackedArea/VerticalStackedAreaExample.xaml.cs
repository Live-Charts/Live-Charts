using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Uwp;
using System;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace UWP.CartesianChart.StackedArea
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class VerticalStackedAreaExample : Page
    {
        public VerticalStackedAreaExample()
        {
            InitializeComponent();

            SeriesCollection = new SeriesCollection
            {
                new VerticalStackedAreaSeries
                {
                    Values = new ChartValues<ObservableValue>
                    {
                        new ObservableValue(5),
                        new ObservableValue(3),
                        new ObservableValue(6),
                        new ObservableValue(2)
                    },
                    DataLabels = true
                },
                new VerticalStackedAreaSeries
                {
                    Values = new ChartValues<ObservableValue>
                    {
                        new ObservableValue(4),
                        new ObservableValue(1),
                        new ObservableValue(7),
                        new ObservableValue(9)
                    },
                    DataLabels = true
                },
                new VerticalStackedAreaSeries
                {
                    Values = new ChartValues<ObservableValue>
                    {
                        new ObservableValue(2),
                        new ObservableValue(8),
                        new ObservableValue(2),
                        new ObservableValue(9)
                    },
                    DataLabels = true
                }
            };

            DataContext = this;
        }

        public SeriesCollection SeriesCollection { get; set; }
        public StackMode StackMode { get; set; }

        private void UpdateAllOnClick(object sender, RoutedEventArgs e)
        {
            var r = new Random();

            foreach (var series in SeriesCollection)
            {
                foreach (var observable in series.Values.Cast<ObservableValue>())
                {
                    observable.Value = r.Next(1, 10);
                }
            }
        }

        private void ChangeStackModeOnClick(object sender, RoutedEventArgs e)
        {
            foreach (var series in SeriesCollection.Cast<VerticalStackedAreaSeries>())
            {
                series.StackMode = series.StackMode == StackMode.Percentage
                    ? StackMode.Values
                    : StackMode.Percentage;
            }
        }
    }
}
