using System.Diagnostics;
using System.Windows.Media;
using System.Windows.Navigation;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;

namespace Wpf.CartesianChart
{
    /// <summary>
    /// Interaction logic for TypesTesting.xaml
    /// </summary>
    public partial class CustomTypesPlotting
    {
        public CustomTypesPlotting()
        {
            InitializeComponent();

            SeriesCollection = new SeriesCollection
            {
                new LineSeries
                {
                    Values = new ChartValues<double> {3d, 6d, 2d, 8d},
                    Fill = Brushes.Transparent,
                    PointGeometrySize = 0,
                    LineSmoothness = 1
                },
                new ColumnSeries
                {
                    Values = new ChartValues<int> {6, 8, 2, 5},
                    Fill = new SolidColorBrush(Color.FromRgb(238, 153, 153)),
                    StrokeThickness = 0
                },
                new ColumnSeries
                {
                    Values = new ChartValues<decimal> {3m, 2m, 3m, 8m},
                    Fill = new SolidColorBrush(Color.FromRgb(254, 223, 129)),
                    StrokeThickness = 0
                },
                new LineSeries
                {
                    Values = new ChartValues<ObservableValue>
                    {
                        new ObservableValue(3),
                        new ObservableValue(2),
                        new ObservableValue(3),
                        new ObservableValue(2)
                    },
                    Fill = Brushes.Transparent,
                    PointGeometrySize = 0,
                    LineSmoothness = 1
                }
            };

            DataContext = this;
        }

        public SeriesCollection SeriesCollection { get; set; }

        private void OnLinkRequest(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }
    }
}
