using System.Windows;
using LiveCharts;

namespace ChartsTest.BarExamples.RotatedBar
{
    /// <summary>
    /// Interaction logic for RotatedLine.xaml
    /// </summary>
    public partial class RotatedBar
    {
        public RotatedBar()
        {
            InitializeComponent();

            SeriesCollection =
                new SeriesCollection(new SeriesConfiguration<double>().X(value => value))
                {
                    new BarSeries
                    {
                        Title = "inverted series",
                        Values = new double[] {10, 15, 18, 20, 15, 13}.AsChartValues()
                    },
                    new BarSeries
                    {
                        Title = "inverted series 2",
                        Values = new double[] {4, 8, 19, 19, 16, 12}.AsChartValues()
                    },
                    new LineSeries
                    {
                        Title = "inverted line series",
                        Values = new double[] {10, 15, 18, 20, 15, 13}.AsChartValues()
                    }
                };

            DataContext = this;
        }

        public SeriesCollection SeriesCollection { get; set; }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            //this is just to force redraw everytime this view loads
            Chart.ClearAndPlot();
        }
    }
}
