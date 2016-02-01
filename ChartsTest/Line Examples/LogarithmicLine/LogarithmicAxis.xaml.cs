using System;
using System.Globalization;
using System.Windows;
using System.Windows.Media;
using LiveCharts;

namespace ChartsTest.Line_Examples.LogarithmicLine
{
    /// <summary>
    /// Interaction logic for LogarithmicAxis.xaml
    /// </summary>
    public partial class LogarithmicAxis
    {
        public LogarithmicAxis()
        {
            InitializeComponent();

            //we create a configuration to map our values type, in this case System.Windows.Point
            var config = new SeriesConfiguration<Point>()
                .X(point => Math.Log(point.X, 10)) // we use log10(point.X) as X
                .Y(point => point.Y); // we use point.Y as the Y of our chart (amm.... yes, we are so f* smart!)

            //we pass the config to the SeriesCollection constructor, or you can use Series.Setup(config)
            Series = new SeriesCollection(config)
            {
                new LineSeries
                {
                    Values = new ChartValues<Point>
                    {
                        new Point(1, 10),
                        new Point(10, 15),
                        new Point(100, 29),
                        new Point(1000, 38),
                        new Point(10000, 45),
                        new Point(100000, 55)
                    },
                    Fill = Brushes.Transparent
                }
            };

            //to display labels we convert back from log
            //this is just the inverse operation 
            XFormatter = x =>
            {
                return Math.Pow(10, x).ToString();
            };

            DataContext = this;
        }

        public SeriesCollection Series { get; set; }
        public Func<double, string> XFormatter { get; set; }

        private void LogarithmicAxis_OnLoaded(object sender, RoutedEventArgs e)
        {
            //this is only to see animation everytime you load this view.
            Chart.ClearAndPlot();
        }
    }
}
