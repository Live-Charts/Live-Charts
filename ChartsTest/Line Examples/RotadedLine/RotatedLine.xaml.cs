using System.Windows;
using LiveCharts;

namespace ChartsTest.Line_Examples.RotadedLine
{
    /// <summary>
    /// Interaction logic for RotatedLine.xaml
    /// </summary>
    public partial class RotatedLine
    {
        public RotatedLine()
        {
            InitializeComponent();

            SeriesCollection =
                new SeriesCollection(new SeriesConfiguration<double>().X(value => value))
                {
                    new LineSeries
                    {
                        Title = "inverted series",
                        Values = new double[] {10, 15, 18, 20, 15, 0, -3, -2}.AsChartValues()
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
