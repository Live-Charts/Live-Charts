using LiveCharts;
using LiveCharts.Configurations;
using LiveCharts.Defaults;
using LiveCharts.Definitions.Charts;
using LiveCharts.Uwp;
using System;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace UWP.CartesianChart.LogarithmScale
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LogarithmScaleExample : Page
    {
        public LogarithmScaleExample()
        {
            InitializeComponent();

            SeriesCollection = new SeriesCollection(Mappers.Xy<ObservablePoint>()
                .X(point => Math.Log10(point.X))
                .Y(point => point.Y))
            {
                new LineSeries
                {
                    Values = new ChartValues<ObservablePoint>
                    {
                        new ObservablePoint(1, 5),
                        new ObservablePoint(10, 6),
                        new ObservablePoint(100, 4),
                        new ObservablePoint(1000, 2),
                        new ObservablePoint(10000, 8),
                        new ObservablePoint(100000, 2),
                        new ObservablePoint(1000000, 9),
                        new ObservablePoint(10000000, 8)
                    }
                }
            };

            Formatter = value => Math.Pow(10, value).ToString("N");

            DataContext = this;
        }

        public SeriesCollection SeriesCollection { get; set; }
        public Func<double, string> Formatter { get; set; }
        public ISeparatorView CleanSeparator { get; set; } = DefaultAxes.CleanSeparator;
    }
}
