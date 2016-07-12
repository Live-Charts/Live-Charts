using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;

namespace Wpf.CartesianChart.FullyResponsive
{
    public partial class FullyResponsiveExample : UserControl
    {
        public FullyResponsiveExample()
        {
            InitializeComponent();

            MyValues = new ChartValues<ObservableValue>
            {
                new ObservableValue(5),
                new ObservableValue(7),
                new ObservableValue(8),
                new ObservableValue(3)
            };

            var lineSeries = new LineSeries
            {
                Values = MyValues,
                StrokeThickness = 4,
                Fill = Brushes.Transparent,
                PointGeometrySize = 0,
                DataLabels = true
            };

            SeriesCollection = new SeriesCollection {lineSeries};

            DataContext = this;
        }

        public ChartValues<ObservableValue> MyValues { get; set; }
        public SeriesCollection SeriesCollection { get; set; }

        private void AddPointOnClick(object sender, RoutedEventArgs e)
        {
            var r = new Random();
            MyValues.Add(new ObservableValue(r.Next(-20, 20)));
        }

        private void InsertPointOnClick(object sender, RoutedEventArgs e)
        {
            var r = new Random();
            if (MyValues.Count > 3)
                MyValues.Insert(2, new ObservableValue(r.Next(-20, 20)));
        }

        private void RemovePointOnClick(object sender, RoutedEventArgs e)
        {
            MyValues.RemoveAt(0);
        }

        private void AddSeriesOnClick(object sender, RoutedEventArgs e)
        {
            //Yes it also listens for series changes
            var r = new Random();

            var c = SeriesCollection[0].Values.Count;

            var val = new ChartValues<ObservableValue>();

            for (int i = 0; i < c; i++)
            {
                val.Add(new ObservableValue(r.Next(-20, 20)));
            }

            SeriesCollection.Add(new LineSeries
            {
                Values = val,
                StrokeThickness = 4,
                Fill = Brushes.Transparent,
                PointGeometrySize = 0
            });
        }

        private void RemoveSeriesOnClick(object sender, RoutedEventArgs e)
        {
            var s = SeriesCollection.Where(x => x.Values != MyValues).ToList();
            if (s.Count > 0) SeriesCollection.RemoveAt(1);
        }

        private void MoveAllOnClick(object sender, RoutedEventArgs e)
        {
            var r = new Random();

            foreach (var observable in MyValues)
            {
                observable.Value = r.Next(-20, 20);
            }
        }
    }
}
