using System.Windows;
using LiveCharts;

namespace ChartsTest.Line_Examples.Basic
{
    public partial class BasicLine
    {
        public BasicLine()
        {
            InitializeComponent();

            //we create a new SeriesCollection
            Series = new SeriesCollection();

            //create some LineSeries
            var charlesSeries = new LineSeries
            {
                Title = "Charles",
                Values = new ChartValues<double> {10, 5, 7, 5, 7, 8}
            };

            var jamesSeries = new LineSeries
            {
                Title = "James",
                Values = new ChartValues<double> {5, 6, 9, 10, 11, 9},
                Visibility = Visibility.Hidden
            };

            //add our series to our SeriesCollection
            Series.Add(charlesSeries);
            Series.Add(jamesSeries);

            //that's it, LiveCharts is ready and listening for your data changes.
            DataContext = this;
        }

        public SeriesCollection Series { get; set; }

        private void BasicLine_OnLoaded(object sender, RoutedEventArgs e)
        {
            //this is just to see animation everytime you click next
            Chart.ClearAndPlot();
        }
    }

}
