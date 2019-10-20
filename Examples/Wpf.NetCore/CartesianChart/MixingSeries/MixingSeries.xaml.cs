using System;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Navigation;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;

namespace Wpf.CartesianChart.MixingSeries
{
    /// <summary>
    /// Interaction logic for MixingSeries.xaml
    /// </summary>
    public partial class MixingTypes : UserControl
    {
        public MixingTypes()
        {
            InitializeComponent();

            LineSeries = new LineSeries
            {
                Values = new ChartValues<ObservableValue>
                {
                    new ObservableValue(5),
                    new ObservableValue(7),
                    new ObservableValue(2),
                    new ObservableValue(3)
                },
                PointForeground = new SolidColorBrush(Color.FromRgb(50,50,50)),
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
                    new ObservableValue(3)
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

        private void OnLinkRequest(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }

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
