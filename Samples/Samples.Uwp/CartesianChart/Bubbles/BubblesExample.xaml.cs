using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Uwp;
using System;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace UWP.CartesianChart.Bubbles
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class BubblesExample : Page
    {
        public BubblesExample()
        {
            InitializeComponent();

            SeriesCollection = new SeriesCollection
            {
                new ScatterSeries
                {
                    Values = new ChartValues<ScatterPoint>
                    {
                        new ScatterPoint(5, 5, 20),
                        new ScatterPoint(3, 4, 80),
                        new ScatterPoint(7, 2, 20),
                        new ScatterPoint(2, 6, 60),
                        new ScatterPoint(8, 2, 70)
                    },
                    MinPointShapeDiameter = 15,
                    MaxPointShapeDiameter = 45
                },
                new ScatterSeries
                {
                    Values = new ChartValues<ScatterPoint>
                    {
                        new ScatterPoint(7, 5, 1),
                        new ScatterPoint(2, 2, 1),
                        new ScatterPoint(1, 1, 1),
                        new ScatterPoint(6, 3, 1),
                        new ScatterPoint(8, 8, 1)
                    },
                    PointGeometry = DefaultGeometries.Triangle,
                    MinPointShapeDiameter = 15,
                    MaxPointShapeDiameter = 45
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
                foreach (var bubble in series.Values.Cast<ScatterPoint>())
                {
                    bubble.X = r.NextDouble() * 10;
                    bubble.Y = r.NextDouble() * 10;
                    bubble.Weight = r.NextDouble() * 10;
                }
            }
        }
    }
}
