using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Uwp;
using System;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace UWP.CartesianChart.MixingSeries
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MixingSeries : Page
    {
        public MixingSeries()
        {
            InitializeComponent();

            LineSeries = new LineSeries
            {
                Values = new ChartValues<ObservableValue>
                {
                    new ObservableValue(5),
                    new ObservableValue(7),
                    new ObservableValue(2),
                    new ObservableValue(3),
                },
                AreaLimit = 0
            };

            ScatterSeries = new ScatterSeries
            {
                Values = new ChartValues<ScatterPoint>
                {
                    new ScatterPoint(0, 2, 10),
                    new ScatterPoint(1, 1, 2),
                    new ScatterPoint(2, 3, 7),
                    new ScatterPoint(3, 4, 9)
                }
            };

            ColumnSeries = new ColumnSeries
            {
                Values = new ChartValues<ObservableValue>
                {
                    new ObservableValue(5),
                    new ObservableValue(7),
                    new ObservableValue(2),
                    new ObservableValue(3),
                }
            };

            SeriesCollection = new SeriesCollection
            {
                LineSeries,
                ScatterSeries,
                ColumnSeries
            };

            DataContext = this;
        }

        public ScatterSeries ScatterSeries { get; set; }
        public LineSeries LineSeries { get; set; }
        public ColumnSeries ColumnSeries { get; set; }
        public SeriesCollection SeriesCollection { get; set; }

        private void UpdateAllOnClick(object sender, RoutedEventArgs e)
        {
            var r = new Random();

            foreach (var value in LineSeries.Values.Cast<ObservableValue>())
            {
                value.Value = r.Next(-20, 20);
            }
            foreach (var value in ColumnSeries.Values.Cast<ObservableValue>())
            {
                value.Value = r.Next(-20, 20);
            }
            var i = 0;
            foreach (var value in ScatterSeries.Values.Cast<ScatterPoint>())
            {
                value.X = i;
                value.Y = r.Next(-20, 20);
                value.Weight = r.Next(-20, 20);
                i++;
            }
        }
    }
}
