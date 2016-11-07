using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Uwp;
using Windows.UI;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace UWP.CartesianChart.TypesSupport
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class TypesPlotting : Page
    {
        public TypesPlotting()
        {
            InitializeComponent();

            SeriesCollection = new SeriesCollection
            {
                new LineSeries
                {
                    Values = new ChartValues<double> {3d, 6d, 2d, 8d},
                    Fill = new SolidColorBrush(Colors.Transparent),
                    PointGeometrySize = 0,
                    LineSmoothness = 1
                },
                new ColumnSeries
                {
                    Values = new ChartValues<int> {6, 8, 2, 5},
                    Fill = new SolidColorBrush(Color.FromArgb(255, 238, 153, 153)),
                    StrokeThickness = 0
                },
                new ColumnSeries
                {
                    Values = new ChartValues<decimal> {3m, 2m, 3m, 8m},
                    Fill = new SolidColorBrush(Color.FromArgb(255, 254, 223, 129)),
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
                    Fill = new SolidColorBrush(Windows.UI.Colors.Transparent),
                    PointGeometrySize = 0,
                    LineSmoothness = 1
                }
            };

            DataContext = this;
        }

        public string[] Labels { get; set; } = new[] { "Jan", "Feb", "Mar", "Apr", "May" };

        public Separator CleanSeparator { get; set; } = DefaultAxes.CleanSeparator;

        public SeriesCollection SeriesCollection { get; set; }
    }
}
