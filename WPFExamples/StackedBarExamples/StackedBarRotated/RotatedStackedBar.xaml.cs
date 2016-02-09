using System.Windows;
using System.Windows.Media;
using LiveCharts;

namespace ChartsTest.StackedBarExamples.StackedBarRotated
{
    /// <summary>
    /// Interaction logic for RotatedStackedBar.xaml
    /// </summary>
    public partial class RotatedStackedBar
    {
        public RotatedStackedBar()
        {
            InitializeComponent();

            var config = new SeriesConfiguration<double>().X(val => val);

            SeriesCollection = new SeriesCollection(config)
            {
                new StackedBarSeries
                {
                    Title = "Stacked Serie 1",
                    Values = new double[] {3,6,2,7}.AsChartValues(),
                    DataLabels = true
                },
                new StackedBarSeries
                {
                    Title = "Stacked Serie 1",
                    Values = new double[] {6,3,5,2}.AsChartValues(),
                    DataLabels = true
                },
                new LineSeries
                {
                    Title = "Line Series",
                    Values = new double[] {10, 11, 8, 9}.AsChartValues(),
                    Fill = Brushes.Transparent
                }
            };

            DataContext = this;
        }

        public SeriesCollection SeriesCollection { get; set; }

        private void RotatedStackedBar_OnLoaded(object sender, RoutedEventArgs e)
        {
            //this line is only to animate the chart everytime view changes its content.
            Chart.Update();
        }
    }
}
