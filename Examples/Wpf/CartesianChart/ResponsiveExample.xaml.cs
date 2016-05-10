using System;
using System.Linq;
using System.Windows;
using LiveCharts;
using LiveCharts.Wpf;

namespace Wpf.CartesianChart
{
    /// <summary>
    /// Interaction logic for ResponsiveExample.xaml
    /// </summary>
    public partial class ResponsiveExample
    {
        public ResponsiveExample()
        {
            InitializeComponent();

            MyValues = new ChartValues<double>();
            MyValues.Add(5);
            MyValues.Add(7);
            MyValues.Add(1);
            MyValues.Add(3);

            var lineSeries = new VerticalStackedAreaSeries {StackMode = StackMode.Percentage};
            lineSeries.Values = MyValues;

            SeriesCollection = new SeriesCollection();
            SeriesCollection.Add(lineSeries);

            var stacked = new VerticalStackedAreaSeries()
            {
                Values = new ChartValues<double> { 4,5,7,8},
                StackMode = StackMode.Percentage
            };
            var stacked2 = new VerticalStackedAreaSeries
            {
                Values = new ChartValues<double> { 4,7,3,1 },
                StackMode = StackMode.Percentage
            };

            SeriesCollection.Add(stacked);
            SeriesCollection.Add(stacked2);

            DataContext = this;
        }

        public ChartValues<double> MyValues { get; set; }
        public SeriesCollection SeriesCollection { get; set; }

        private void AddPointOnClick(object sender, RoutedEventArgs e)
        {
            var r = new Random();
            MyValues.Add(r.Next(-20, 20));
        }

        private void RemovePointOnClick(object sender, RoutedEventArgs e)
        {
            MyValues.RemoveAt(0);
        }

        private void AddSeriesOnClick(object sender, RoutedEventArgs e)
        {
            //Yes it also listens for series changes
            var r = new Random();
            SeriesCollection.Add(new LineSeries
            {
                Values = new ChartValues<double> {r.Next(-20, 20), r.Next(-20, 20), r.Next(-20, 20), r.Next(-20, 20)}
            });
        }

        private void RemoveSeriesOnClick(object sender, RoutedEventArgs e)
        {
            var s = SeriesCollection.Where(x => x.Values != MyValues).ToList();
            if (s.Count > 0) SeriesCollection.RemoveAt(1);
        }
    }
}
