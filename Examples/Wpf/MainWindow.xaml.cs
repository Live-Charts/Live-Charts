using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using LiveCharts;
using LiveCharts.Wpf;

namespace Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();

            SeriesCollection = new SeriesCollection()
            {
                new ScatterSeries
                {
                    Values = new ChartValues<ScatterPoint>
                    {
                        new ScatterPoint(10, 5, 4),
                        new ScatterPoint(13, 5, 8),
                        new ScatterPoint(12, 3, 7),
                        new ScatterPoint(8, 8, 8)
                    }
                },
                new ScatterSeries
                {
                    Values = new ChartValues<ScatterPoint>
                    {
                        new ScatterPoint(10, 5, 4),
                        new ScatterPoint(13, 5, 8),
                        new ScatterPoint(12, 3, 7),
                        new ScatterPoint(8, 8, 8)
                    }
                },
                new ScatterSeries
                {
                    Values = new ChartValues<ScatterPoint>
                    {
                        new ScatterPoint(10, 5, 4),
                        new ScatterPoint(13, 5, 8),
                        new ScatterPoint(12, 3, 7),
                        new ScatterPoint(8, 8, 8)
                    }
                }
            };

            DataContext = this;
        }

        public SeriesCollection SeriesCollection { get; set; }

        private void Chart_OnDataClick(object sender, ChartPoint chartPoint)
        {
            MessageBox.Show("Hello!");
        }

        private void AddButtonOnClick(object sender, RoutedEventArgs e)
        {
            var r = new Random();

            foreach (var series in SeriesCollection)
            {
                var next = r.NextDouble();
                series.Values.Add(new ScatterPoint(r.NextDouble()*15, r.NextDouble()*15, r.NextDouble()*15));
            }
        }

        private void RemoveButtonOnClick(object sender, RoutedEventArgs e)
        {
            var r = new Random();

            foreach (var series in SeriesCollection)
            {
                if (series.Values.Count > 0)
                    series.Values.RemoveAt(0);
            }
        }

        private void AddSeriesOnClick(object sender, RoutedEventArgs e)
        {
            var c = SeriesCollection.Count > 0 ? SeriesCollection[0].Values.Count : 5;

            var values = new List<double>();
            var r = new Random();

            for (var i = 0; i < c; i++)
            {
                values.Add(r.NextDouble()*128);
            }

            SeriesCollection.Add(new LineSeries
            {
                Values = values.AsChartValues()
            });
        }

        private void RemoveSeriesOnClick(object sender, RoutedEventArgs e)
        {
            var r = new Random();

            if (SeriesCollection.Count > 0)
                SeriesCollection.RemoveAt(r.Next(0, SeriesCollection.Count));
        }

        private void RandomizeOnClick(object sender, RoutedEventArgs e)
        {
            var r = new Random();
            foreach (var series in SeriesCollection)
            {
                foreach (var value in series.Values.Cast<ScatterPoint>())
                {
                    value.X = r.NextDouble()*20;
                    value.Y = r.NextDouble()*20;
                    value.Weight = r.NextDouble()*20;
                }
            }
        }
    }
}
