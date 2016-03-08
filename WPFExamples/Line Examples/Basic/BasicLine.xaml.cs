using System;
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
                Values = new ChartValues<double> {5, 6, 9, 10, 11, 9}
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
            Chart.Update();
        }

        private void RemovePointsOnClick(object sender, RoutedEventArgs e)
        {
            //Remove any point from any series and chart will update
            foreach (var series in Series)
            {
                if (series.Values.Count > 0) series.Values.RemoveAt(0);
            }
        }

        private void AddPointsOnClick(object sender, RoutedEventArgs e)
        {
            //Add a point to any series, and chart will update
            var r = new Random();

            foreach (var series in Series)
            {
                series.Values.Add((double) r.Next(0, 15));
            }
        }

        private void RemoveSeriesOnClick(object sender, RoutedEventArgs e)
        {
            //Remove any series
            if (Series.Count > 0) Series.RemoveAt(0);
        }

        private void AddSeriesOnClick(object sender, RoutedEventArgs e)
        {
            //Ad any series to your chart
            var someRandomValues = new ChartValues<double>();

            var r = new Random();
            var count = Series.Count > 0 ? Series[0].Values.Count : 5;

            for (int i = 0; i < count; i++)
            {
                someRandomValues.Add(r.Next(0,15));
            }

            var someNewSeries = new LineSeries
            {
                Title = "Some Random Series",
                Values = someRandomValues
            };

            Series.Add(someNewSeries);
        }
    }

}
