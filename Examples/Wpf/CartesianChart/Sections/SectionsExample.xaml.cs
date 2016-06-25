using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;

namespace Wpf.CartesianChart.Sections
{
    public partial class SectionsExample : UserControl
    {
        public SectionsExample()
        {
            InitializeComponent();

            SeriesCollection = new SeriesCollection
            {
                new LineSeries
                {
                    Values = new ChartValues<ObservableValue>
                    {
                        new ObservableValue(3),
                        new ObservableValue(5),
                        new ObservableValue(2),
                        new ObservableValue(7),
                        new ObservableValue(7),
                        new ObservableValue(4)
                    },
                    PointGeometrySize = 0,
                    StrokeThickness = 4,
                    Fill = Brushes.Transparent
                },
                new LineSeries
                {
                    Values = new ChartValues<ObservableValue>
                    {
                        new ObservableValue(3),
                        new ObservableValue(4),
                        new ObservableValue(6),
                        new ObservableValue(8),
                        new ObservableValue(7),
                        new ObservableValue(5)
                    },
                    PointGeometrySize = 0,
                    StrokeThickness = 4,
                    Fill = Brushes.Transparent
                }
            };

            DataContext = this;
        }

        public SeriesCollection SeriesCollection { get; set; }

        private void UpdateAllOnClick(object sender, RoutedEventArgs e)
        {
            var r = new Random();

            foreach (var series in SeriesCollection)
            {
                foreach (var observable in series.Values.Cast<ObservableValue>())
                {
                    observable.Value = r.Next(0, 10);
                }
            }
        }
    }
}
