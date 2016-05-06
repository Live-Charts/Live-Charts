using System;
using System.Collections.Generic;
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

            SeriesCollection = new SeriesCollection
            {
                new LineSeries
                {
                    Values = new ChartValues<double> {2, 3, 5, 2, double.NaN}
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
                //series.Values.Add((next > .85 ? double.NaN : next*128));
                series.Values.Add(r.NextDouble()*128);
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
    }
}
