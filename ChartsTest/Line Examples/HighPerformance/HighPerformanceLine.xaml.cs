using System;
using System.Windows;
using LiveCharts;
using LiveCharts.Optimizations;

namespace ChartsTest.Line_Examples.HighPerformance
{
    /// <summary>
    /// Interaction logic for HighPerformanceLine.xaml
    /// </summary>
    public partial class HighPerformanceLine
    {
        public HighPerformanceLine()
        {
            InitializeComponent();

            //first you need to install LiveCharts.Optimizations
            //from Nuget:
            //Install-Package LiveCharts.Optimizations

            //low quality is actually really accurate
            //it could only have a +-3 pixels error
            //default is low quality.
            var highPerformanceMethod = new RegularX<double>().WithQuality(DataQuality.Low);

            var config = new SeriesConfiguration<double>()
                .X((val, index) => index)
                .Y(val => val)
                .HasHighPerformanceMethod(highPerformanceMethod);

            Series = new SeriesCollection(config);

            var line = new LineSeries {Values = new ChartValues<double>(), PointRadius = 0};

            var r = new Random();
            var trend = 0d;

            for (var i = 0; i < 1000000; i++)
            {
                if (i%1000 == 0) trend += r.Next(-500, 500);
                line.Values.Add(trend + r.Next(-10, 10));
            }

            Series.Add(line);

            var now = DateTime.Now.ToOADate();
            XFormat = val => Math.Round(val).ToString("N0");//DateTime.FromOADate(now + val/100).ToShortDateString();
            YFormat = val => Math.Round(val) + " ms";

            DataContext = this;
        }

        public SeriesCollection Series { get; set; }
        public Func<double, string> XFormat { get; set; }
        public Func<double, string> YFormat { get; set; }

        private void HighPerformanceLine_OnLoaded(object sender, RoutedEventArgs e)
        {
            //this is only to force animation everytime you change the current view.
            Chart.ClearAndPlot();
        }
    }
}
